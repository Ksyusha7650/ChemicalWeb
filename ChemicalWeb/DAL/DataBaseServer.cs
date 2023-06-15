using MySql.Data.MySqlClient;

namespace ChemicalWeb.DAL;

public class DataBaseServer
{
    public static List<DataBaseWorker.MaterialInfo> GetMaterialsInfoForLabel(string materialName)
    {
        try
        {
            return DataBaseWorker.GetMaterialsInfoForLabel(materialName);
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static List<DataBaseWorker.MaterialInfo> GetCoefficientsInfoForLabel(string materialName)
    {
        try
        {
            return DataBaseWorker.GetCoefficientsInfoForLabel(materialName);
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}