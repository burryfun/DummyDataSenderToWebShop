using DummyDataSenderToWebShop;
using System.Text.RegularExpressions;


//For all brands:
//     Add brand...
//         Add BrandImage

//For all smartphones:
//     Add smartphone...
//         Add SmartphoneImage


string APP_PATH = Directory.GetCurrentDirectory();
string APP_ROOT_PATH = APP_PATH.Replace("\\bin\\Debug\\net6.0", "");

string DATA_ROOT_PATH = APP_ROOT_PATH + "/data";

const string URL = "http://localhost:5054/api";
const string EMAIL = "admin@admin.com";
const string PASSWORD = "admin";

// Create instance of DataSender
DataSender sender = await DataSender.Create(URL, EMAIL, PASSWORD);

try
{
    if (!Directory.Exists(DATA_ROOT_PATH))
    {
        throw new Exception("Data folder not exists");
    }

    string[] brandDirectories = Directory.GetDirectories(DATA_ROOT_PATH);

    foreach (string brandPath in brandDirectories)
    {
        if (brandPath == DATA_ROOT_PATH + "\\" + "logo")
        {
            continue;
        }

        string brandName = brandPath.Replace(DATA_ROOT_PATH + "\\", "");
        Guid brandGuid = Guid.NewGuid();
        // Send brand to API
        var brandResponse = await sender.SendBrand(brandGuid, brandName);

        // Send brandLogo to API
        string logoPath = Path.Combine(DATA_ROOT_PATH, "logo", $"{brandName}.svg");
        var brandImageResponse = await sender.SendBrandImage(brandGuid, logoPath);

        Console.WriteLine($"{brandName}: \n");
        
        string[] smartphoneImagePaths = Directory.GetFiles(brandPath);
        foreach(string smartphoneImagePath in smartphoneImagePaths)
        {
            string smartphoneImageTitle = smartphoneImagePath.Replace(DATA_ROOT_PATH + $"\\{brandName}\\", "");
            string smartphoneName = GetSmartphoneNameFromImageTitle(smartphoneImageTitle);
            Console.WriteLine($"\t{smartphoneName}");
        }

    }

} catch (Exception ex)
{
    Console.WriteLine(ex);
}

string GetSmartphoneNameFromImageTitle(string imageTitle)
{
    string pattern = @"смартфон |фото, technodom.kz.jpg";
    string target = "";
    Regex regex = new Regex(pattern);

    string smartphoneName = regex.Replace(imageTitle, target);

    return smartphoneName;
}
