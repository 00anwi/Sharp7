using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace Sharp7
{
    public class VariableAddress
    {
        public VariableAddress(S7Area operand, ushort dbNo, DbType type, ushort start, ushort length, byte? bit = null)
        {
            Operand = operand;
            DbNo = dbNo;
            Type = type;
            Start = start;
            Length = length;
            Bit = bit;
        }

        public S7Area Operand { get; }
        public ushort DbNo { get; }
        public ushort Start { get; } 
        public ushort Length { get; }
        public byte? Bit { get; }
        public DbType Type { get; }

        public ushort BufferLength => Type switch
        {
            DbType.String => (ushort)(Length + 2),
            _ => Length
        };

        public override string ToString() =>
        Type switch
        {
            DbType.Boolean => $"{Operand}{DbNo}.{Type}{Start}.{Bit}",
            DbType.String => $"{Operand}{DbNo}.{Type}{Start}.{Length}",
            DbType.Byte => Length == 1 ? $"{Operand}{DbNo}.{Type}{Start}" : $"{Operand}{DbNo}.{Type}{Start}.{Length}",
            _ => $"{Operand}{DbNo}.{Type}{Start}",
        };
    }
}
