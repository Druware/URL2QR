using QRCoder;
using URL2QR;

void showHelp(string option)
{
    Console.WriteLine("Help for " + option);
}

var availableOptions = new AvailableOptions();
availableOptions.Add(
    "-h|--help", 
    "Display the help file, either this over all help, or if there " + 
    "is an argument the help for the specific option.");
availableOptions.Add(
    "-p|--path", 
    "The path for the output file. The default is the current working directory.");
availableOptions.Add(
    "-u|--url", 
    "The url to be encoded. required");
availableOptions.Add(
    "-n|--name", 
    "The output file name to be used: default = qr.png");

var options = new Options(availableOptions);

var optionResults = options.Parse(args);

if (optionResults.Contains("-h"))
{
    showHelp(optionResults.Value("-h"));
    return;
}

// -- 
var path = optionResults.Value("-p") ?? Directory.GetCurrentDirectory();
var fileName = optionResults.Value("-n") ?? "qr.png";
string url = optionResults.Value("-u") ?? throw new Exception("A URL is required");

using var qrGenerator = new QRCodeGenerator();
using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
using var qrCode = new PngByteQRCode(qrCodeData);
var qrCodeImage = qrCode.GetGraphic(20);

File.WriteAllBytes(Path.Combine(path, fileName), qrCodeImage);

Console.WriteLine("QR Code generated:\n\t" + fileName + "\n\tfor: " + url);
