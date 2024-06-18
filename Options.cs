namespace URL2QR;

public class AvailableOptions
{
    private Dictionary<string, string> _internal;

    public AvailableOptions(Dictionary<string, string>? optionSet = null)
    {
        _internal = optionSet ?? new Dictionary<string, string>();
    }

    public void Add(string option, string description)
    {
        _internal[option.ToLower()] = description;
    }

    private string? PrivateValueFor(string arg, Dictionary<string, string> args)
    {
        string? result = null;
        if (args.Keys.Contains(arg.ToLower()))
            result = args[arg.ToLower()];
        return result;
    }
    
    public string? ValueFor(string arg, Dictionary<string, string> args)
    {
        // split the option string, and compare it to the existing options.
        // if found, return a value
        string? result = null;
        
        // not using te arg at the moment.  mistake
        
        foreach (var key in _internal.Keys)
        {
            if (key.Contains('|'))
            {
                // split the key.
                foreach (var k in key.Split("|"))
                    if (k == arg)
                        result = PrivateValueFor(k, args);
            }
            else
                if (key == arg)
                    result = PrivateValueFor(key, args);
        }
        return result;
    }
}

public class OptionsParseResult
{
    private Dictionary<string, string> _internal = new();
    
    public OptionsParseResult() { }

    public void Add(string option, string value)
    {
        _internal[option] = value;
    }

    public bool Contains(string option)
    {
        return _internal.Keys.Contains(option.ToLower());
    }

    public string? Value(string option)
    {
        return _internal.Keys.Contains(option.ToLower()) ? 
            _internal[option.ToLower()] :
            null;
    }
}

public class Options
{
    private AvailableOptions _options;
    private OptionsParseResult? _results;
    
    public Options(AvailableOptions? options = null)
    {
        _options = options ?? new AvailableOptions();
    }

    private Dictionary<string, string> ArgsToDictionary(string[] args)
    {
        var result = new Dictionary<string, string>();
        string? key = null;
        
        foreach (var arg in args)
        {
            if (key != null && arg.StartsWith('-'))
            {
                result[key] = "";
                key = null;
                continue;
            }
            
            if (arg.StartsWith('-'))
            {
                key = arg;
                continue;
            }

            if (key == null) key = "";
            
            result[key] = arg;
        }

        return result;
    }
    
    public OptionsParseResult Parse(string[] args)
    {
        _results = new OptionsParseResult();
        var dict = ArgsToDictionary(args);

        // implement the parse and then populate the results.
        foreach (var key in dict.Keys)
        {
            var value = _options.ValueFor(key, dict);
            if (value != null) _results.Add(key, value);
        }
        
        return _results;
    }
    
}