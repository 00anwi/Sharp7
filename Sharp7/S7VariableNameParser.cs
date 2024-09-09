using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Sharp7
{
    public class S7VariableNameParser
    {
        private static readonly Regex regex = new Regex(@"^(?<operand>(db|di|dq|mk|ct|tm))(?<dbNo>\d*)\.?(?<type>[a-z]+)(?<start>\d+)(\.(?<bitOrLength>\d+))?$",
                                                  RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly IReadOnlyDictionary<string, DbType> types = new Dictionary<string, DbType>(StringComparer.OrdinalIgnoreCase)
    {
        {"bit", DbType.Boolean},

        {"string", DbType.String},

        {"byte", DbType.Byte},
        {"int", DbType.Int16},
        {"uint", DbType.UInt16},
        {"dint", DbType.Int32},
        {"udint", DbType.UInt32},
        {"lint", DbType.Int64},
        {"ulint", DbType.UInt64},

        {"real", DbType.Single},
        {"lreal", DbType.Double},

        {"dbb", DbType.Byte},
        {"dbw", DbType.Int16},
        {"dbx", DbType.Boolean},
        {"dbd", DbType.Int32},

        {"b", DbType.Byte},
        {"d", DbType.Single},
        {"dul", DbType.UInt32},
        {"dulint", DbType.UInt32},
        {"dulong", DbType.UInt32},
        {"s", DbType.String},
        {"w", DbType.Int16},
        {"x", DbType.Boolean},
    };

        public static VariableAddress Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            var match = regex.Match(input);
            if (!match.Success)
                throw new InvalidS7AddressException($"Invalid S7 address \"{input}\". Expect format \"DB<dbNo>.<type><startByte>(.<length>)\".", input);

            var operand = (S7Area)Enum.Parse(typeof(S7Area), match.Groups["operand"].Value, true);

            ushort dbNr = 0;
            if (operand == S7Area.DB)
            {
                // DB operand must have a dbNo
                if (!ushort.TryParse(match.Groups["dbNo"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out dbNr))
                    throw new InvalidS7AddressException($"\"{match.Groups["dbNo"].Value}\" is an invalid DB number in \"{input}\"", input);
            }
            else if (!string.IsNullOrEmpty(match.Groups["dbNo"].Value))
            {
                throw new InvalidS7AddressException($"Operand \"{operand}\" should not have a DB number: \"{input}\"", input);
            }

            if (!ushort.TryParse(match.Groups["start"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var start))
                throw new InvalidS7AddressException($"\"{match.Groups["start"].Value}\" is an invalid start bit in \"{input}\"", input);

            if (!types.TryGetValue(match.Groups["type"].Value, out var type))
                throw new InvalidS7AddressException($"\"{match.Groups["type"].Value}\" is an invalid type in \"{input}\"", input);

            ushort length = type switch
            {
                DbType.Boolean => 1,

                DbType.String => GetLength(),

                DbType.Byte => GetLength(1),

                DbType.Int16 => 2,
                DbType.Int32 => 4,
                DbType.UInt64 => 8,
                DbType.UInt16 => 2,
                DbType.UInt32 => 4,
                DbType.Int64 => 8,

                DbType.Single => 4,
                DbType.Double => 8,
                _ => throw new ArgumentOutOfRangeException($"DbType {type} is not supported")
            };

            switch (type)
            {
                case DbType.Boolean:
                case DbType.String:
                case DbType.Byte:
                    break;
                case DbType.Int16:
                case DbType.UInt16:
                case DbType.Int32:
                case DbType.UInt32:
                case DbType.Int64:
                case DbType.UInt64:
                case DbType.Single:
                case DbType.Double:
                default:
                    if (match.Groups["bitOrLength"].Success)
                        throw new InvalidS7AddressException($"{type} address must not have a length: \"{input}\"", input);
                    break;
            }

            byte? bit = type == DbType.Boolean ? GetBit() : null;


            var s7VariableAddress = new VariableAddress(operand, dbNr, type, start, length, bit);

            return s7VariableAddress;

            ushort GetLength(ushort? defaultValue = null)
            {
                if (!match.Groups["bitOrLength"].Success)
                {
                    if (defaultValue.HasValue)
                        return defaultValue.Value;
                    throw new InvalidS7AddressException($"Variable of type {type} must have a length set. Example \"db12.byte10.3\", found \"{input}\"", input);
                }

                if (!ushort.TryParse(match.Groups["bitOrLength"].Value, out var result))
                    throw new InvalidS7AddressException($"\"{match.Groups["bitOrLength"].Value}\" is an invalid length in \"{input}\"", input);

                return result;
            }

            byte? GetBit()
            {
                if (!match.Groups["bitOrLength"].Success)
                    throw new InvalidS7AddressException($"Variable of type {type} must have a bit number set. Example \"db12.bit10.3\", found \"{input}\"", input);

                if (!byte.TryParse(match.Groups["bitOrLength"].Value, out var result))
                    throw new InvalidS7AddressException($"\"{match.Groups["bitOrLength"].Value}\" is an invalid bit number in \"{input}\"", input);

                if (result > 7)
                    throw new InvalidS7AddressException($"Bit must be between 0 and 7 but is {result} in \"{input}\"", input);

                return result;
            }
        }
    }
}

public abstract class S7Exception : Exception
{
    protected S7Exception(string message) : base(message)
    {
    }

    protected S7Exception(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class InvalidS7AddressException : S7Exception
{
    public InvalidS7AddressException(string message, string input) : base(message)
    {
        Input = input;
    }

    public InvalidS7AddressException(string message, Exception innerException, string input) : base(message, innerException)
    {
        Input = input;
    }

    public string Input { get; }
}