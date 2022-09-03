using DummyDataSenderToWebShop;


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
        string brandName = brandPath.Replace(DATA_ROOT_PATH + "\\", "");
        //var response = await sender.SendBrand(brandName);
        Console.WriteLine(brandName);
    }

} catch (Exception ex)
{
    Console.WriteLine(ex);
}

