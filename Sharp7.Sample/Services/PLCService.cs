using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneOf;

namespace Sharp7.Sample.Services;
internal class PLCService
{
    private S7Client _client;
    private string _ipAddress = "192.168.0.159";
    private int _rack = 0;
    private int _slot = 1;

    public List<VariableAddress> Variables { get; set; } = new();

    public PLCService()
    {
        _client = new S7Client();

        Variables.Add(S7VariableNameParser.Parse("DI.X0.0"));
        Variables.Add(S7VariableNameParser.Parse("DI.X0.1"));
        Variables.Add(S7VariableNameParser.Parse("DB1.DBX0.0"));
        Variables.Add(S7VariableNameParser.Parse("DB1.DBX0.1"));
        Variables.Add(S7VariableNameParser.Parse("DB1.INT2"));
    }

    public async Task<OneOf<bool, string>> Connect()
    {
        var result = await _client.ConnectToAsync(_ipAddress, _rack, _slot);

        return result == 0 ? true : _client.ErrorText(result);
    }

    public async Task Disconnect()
    {
        await _client.DisconnectAsync();
    }


}
