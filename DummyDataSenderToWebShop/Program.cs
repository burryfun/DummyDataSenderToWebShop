using DummyDataSenderToWebShop;
using System.Text.RegularExpressions;


//For all brands:
//     Add brand...
//         Add BrandImage

//For all smartphones:
//     Add smartphone...
//         Add SmartphoneImage


string APP_PATH = Directory.GetCurrentDirectory();

string DATA_ROOT_PATH = APP_PATH + "/data";

const string URL = "http://localhost:5000/api";
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
        if (brandPath == DATA_ROOT_PATH + "/" + "logo")
        {
            continue;
        }

        string brandName = brandPath.Replace(DATA_ROOT_PATH + "/", "");
        Guid brandGuid = Guid.NewGuid();
        // Send brand to API
        var brandResponse = await sender.SendBrand(brandGuid, brandName);

        if (brandResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($"Brand {brandName} successfuly added");
        }

        // Send brandLogo to API
        string logoPath = Path.Combine(DATA_ROOT_PATH, "logo", $"{brandName}.svg");
        var brandImageResponse = await sender.SendBrandImage(brandGuid, logoPath);

        if (brandImageResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($"Brand image for {brandName} successfuly added");
        }

        string[] smartphoneImagePaths = Directory.GetFiles(brandPath);

        foreach(string smartphoneImagePath in smartphoneImagePaths)
        {
            string smartphoneImageTitle = smartphoneImagePath.Replace(DATA_ROOT_PATH + $"\\{brandName}\\", "");
            string smartphoneName = GetSmartphoneNameFromImageTitle(smartphoneImageTitle);

            Guid smartphoneGuid = Guid.NewGuid();

            Random rand = new Random();
            decimal smartphonePrice = rand.Next(500, 2000);
            int smartphoneDiscount = rand.Next(0, 50);

            // Send Smartphone data to API
            var smartphoneResponse = await sender.SendSmartphone(
                smartphoneGuid,
                brandName,
                smartphoneName,
                smartphoneName,
                smartphonePrice,
                smartphoneDiscount
                );

            if (smartphoneResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Smartphone {smartphoneName} successfuly added");
            }

            // Send SmartphoneImage to API
            var smartphoneImageResponse = await sender.SendSmartphoneImage(smartphoneGuid, smartphoneImagePath, brandName);
            
            if (brandResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Smartphone image for {smartphoneName} successfuly added");
            }
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
