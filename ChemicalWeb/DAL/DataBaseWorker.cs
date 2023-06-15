using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ChemicalWeb.DAL; 

public abstract class DataBaseWorker {

    private static readonly string ConnectionString = ConfigurationManager.AppSettings["connectionString"]!;
    
    public record ParameterInfo(
        string Name,
        string Symbol,
        string Unit
    );
    
    public record MaterialInfoNames(
        string MaterialName,
        string ParameterName,
        double Value,
        string Type
    );
    
    public record ParameterInfoWithId(
        int Id,
        string Name,
        string Symbol,
        string Unit
    );
    
    public record MaterialInfoWithId(
        string Name, 
        double Value,
        string Type);
    
    public record MaterialInfo(
        string MaterialType,
        double Value,
        string Unit
    );

    private static MySqlConnection JoinBase() {
        var connection = new MySqlConnection();
        connection.ConnectionString = ConnectionString;
        connection.Open();
        return connection;
    }
    
    // public static void AddParameter(ParameterInfo parameterInfo) {
    //
    //     var query = "INSERT parameter(name, symbol, unit) " +
    //                 $"VALUES ('{parameterInfo.Name}', '{parameterInfo.Symbol}', '{parameterInfo.Unit}');";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    //
    // public static void AddMaterial(string materialType) {
    //
    //     var query = "INSERT material(type) " +
    //                 $"VALUES ('{materialType}');";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    
    // public static void AddMaterialParameter(AddMaterialWindow.Xrecord record) {
    //
    //     var query = "INSERT INTO parameter_material_attr (ID_material, ID_parameter, value, type) " +
    //                 $"VALUES ({record.IdMaterial}, {record.IdParameter}, {record.Value}, '{record.Type}');";
    //     var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    
    // public static void UpdateParameter(int id, ParameterInfo parameterInfo) {
    //
    //     var query = "UPDATE parameter " +
    //                 $"SET name = '{parameterInfo.Name}', symbol = '{parameterInfo.Symbol}', unit = '{parameterInfo.Unit}' " +
    //                 $"WHERE ID_parameter = {id}";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    //
    // public static List<int> GetParametersId() {
    //
    //     const string query = "SELECT ID_parameter FROM parameter;";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = new List<int>();
    //     while (reader.Read()) {
    //         result.Add(reader.GetInt32(0));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    //
    // public static ParameterInfo? GetParameterById(int id) {
    //     var query = "SELECT name, symbol, unit " +
    //                 "FROM parameter " +
    //                 $"WHERE ID_parameter = {id}";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     ParameterInfo? result = null;
    //     while (reader.Read()) {
    //         result = new (reader.GetString(0),reader.GetString(1), reader.GetString(2));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    //
    // public static int GetMaterialIdByType(string type) {
    //     var query = "SELECT ID_material " +
    //                 "FROM material " +
    //                 $"WHERE type = '{type}'";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = 0;
    //     while (reader.Read()) {
    //         result = reader.GetInt32(0);
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    //
    // public static int? GetParameterIdByName(string name) {
    //     var query = "SELECT ID_parameter " +
    //                 "FROM parameter " +
    //                 $"WHERE name = '{name}'";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     int? result = null;
    //     while (reader.Read()) {
    //         result = reader.GetInt32(0);
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    // public static List<MaterialInfoWithId> GetParameterByNameMaterial(string name) {
    //     string query = "SELECT name, value, p.type " +
    //                          "FROM chemical.material c " +
    //                          $"join parameter_material_attr p on c.ID_material = p.ID_material and c.type = \"{name}\" " +
    //                          "join parameter par on p.ID_parameter = par.ID_parameter;";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = new List<MaterialInfoWithId>();
    //     while (reader.Read()) {
    //         result.Add(new (reader.GetString(0), reader.GetDouble(1), reader.GetString(2)));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    public static List<string> GetMaterials() {
    
        const string query = "SELECT type FROM material";
        using var connection = JoinBase();
        var command = new MySqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        var reader = command.ExecuteReader();
        var result = new List<string>();
        while (reader.Read()) {
            result.Add(reader.GetString(0));
        }
        connection.Close();
        
        return result;
    }
    
    public static bool CheckUserData(string? login="", string? password="") {
        login ??= "";
        password ??= "";
        var query = "SELECT 1 " +
                    "FROM users " +
                    $"WHERE login = '{login}' AND password = '{password}';";
        using var connection = JoinBase();
        var command = new MySqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        var reader = command.ExecuteReader();
        var result = reader.Read();
        connection.Close();
        
        return result;
    }
    //
    // public static List<string> GetParametersName() {
    //
    //     const string query = "SELECT name FROM parameter";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = new List<string>();
    //     while (reader.Read()) {
    //         result.Add(reader.GetString(0));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    //
    // public static List<ParameterInfoWithId> GetParameterTable() {
    //
    //     const string query = "SELECT ID_parameter AS \"ИД\", " +
    //                          "name AS \"Название\", " +
    //                          "symbol AS \"Условное_обозначение\", " +
    //                          "unit AS \"Единица_измерения\" " +
    //                          "FROM parameter;";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = new List<ParameterInfoWithId>();
    //     while (reader.Read()) {
    //         result.Add(new (reader.GetInt32(0), reader.GetString(1),
    //                             reader.GetString(2), reader.GetString(3)));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    //
    // public static List<MaterialInfoNames> GetMaterialTable() {
    //
    //     const string query = "SELECT m.type, p.name, value, parameter_material_attr.type " +
    //                          "FROM parameter_material_attr " +
    //                          "JOIN material m on parameter_material_attr.ID_material = m.ID_material " +
    //                          "JOIN parameter p on parameter_material_attr.ID_parameter = p.ID_parameter;";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = new List<MaterialInfoNames>();
    //     while (reader.Read()) {
    //         result.Add(new (reader.GetString(0), reader.GetString(1),
    //             reader.GetDouble(2), reader.GetString(3)));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
    //
    // public static void DeleteMaterial(string materialType) {
    //     var query = $"DELETE FROM material WHERE type = \"{materialType}\";";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    //
    // public static void DeleteParameter(int id) {
    //     var query = $"DELETE FROM parameter WHERE ID_parameter = {id};";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    //
    // public static void DeleteMaterialInfo(string materialType) {
    //
    //     var query = "DELETE " +
    //                 "FROM parameter_material_attr " +
    //                 "WHERE ID_material = (" +
    //                 "SELECT ID_material " +
    //                 "FROM material " +
    //                 $"WHERE type = \"{materialType}\");";
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.CommandText = query;
    //     command.ExecuteReader();
    //     connection.Close();
    // }
    //
    
    //
    /*public static List<MaterialInfo> GetMaterialsInfo(string parameter) {
        
        var query = $"SELECT material.type, value AS `{parameter}`, unit " +
                    "FROM material " +
                    "JOIN parameter_material_attr pma on material.ID_material = pma.ID_material " +
                    "JOIN parameter p ON pma.ID_parameter = p.ID_parameter " +
                    $"WHERE name = \"{parameter}\"";
        using var connection = JoinBase();
        var command = new MySqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        var reader = command.ExecuteReader();
        var result = new List<MaterialInfo>();
        while (reader.Read()) {
            result.Add(new (reader.GetString(0),reader.GetDouble(1), reader.GetString(2)));
        }
        connection.Close();
        
        return result;
    }*/
    //
    
    public static string ClickExport() {
        //@TODO: поменять -p в методе и в app.config
        try {

            string commands = @"cd C:\Program Files\MySQL\MySQL Server 8.0\bin && mysqldump.exe -h127.0.0.1 " +
                              @$"-uroot -p04042002Mm! chemical > {Environment.CurrentDirectory}\dump.sql";
            string batPath = Path.Combine(Path.GetTempPath(), "dump.bat");
            File.WriteAllText(batPath, commands);
            Process cmd = Process.Start(batPath);
            cmd.WaitForExit();
            File.Delete(batPath);
            return Path.GetTempPath();
        }
        catch
        {
            Console.WriteLine("Ошибка");
            return "";
        }
    }
    
    public static bool ClickImport() {
        //@TODO: поменять -p в методе и в app.config
        try {
            string commands = @$"cd C:\Program Files\MySQL\MySQL Server 8.0\bin && mysql -uroot -p04042002Mm! chemical < {Environment.CurrentDirectory}\dump.sql";
            string batPath = Path.Combine(Path.GetTempPath(), "dump.bat");
            File.WriteAllText(batPath, commands);
            Process cmd = Process.Start(batPath);
            cmd.WaitForExit();
            File.Delete(batPath);
            return true;
        }
        catch
        {
            Console.WriteLine("Ошибка");
            return false;
        }
    }
    public static List<MaterialInfo> GetMaterialsInfoForLabel(string materialName) {
        
        var query = "SELECT name, value, unit " +
                    "FROM material " +
                    "JOIN parameter_material_attr pma on material.ID_material = pma.ID_material " +
                    "JOIN parameter p on pma.ID_parameter = p.ID_parameter " +
                    $"WHERE material.type = \"{materialName}\" and pma.type = \"Свойство материала\"";
        using var connection = JoinBase();
        var command = new MySqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        var reader = command.ExecuteReader();
        var result = new List<MaterialInfo>();
        while (reader.Read()) {
            result.Add(new (reader.GetString(0),reader.GetDouble(1), reader.GetString(2)));
        }
        connection.Close();
        
        return result;
    }
    
    public static bool Backup(string file)
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            using var cmd = new MySqlCommand();
            using var mb = new MySqlBackup(cmd);
            cmd.Connection = conn;
            conn.Open();
            mb.ExportToFile(file);
            conn.Close();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    public static bool Restore(string file)
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            using var cmd = new MySqlCommand();
            using var mb = new MySqlBackup(cmd);
            cmd.Connection = conn;
            conn.Open();
            mb.ImportFromFile(file);
            conn.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
     public static List<MaterialInfo> GetCoefficientsInfoForLabel(string materialName) {
         
         var query = "SELECT name, value, unit " +
                     "FROM material " +
                     "JOIN parameter_material_attr pma on material.ID_material = pma.ID_material " +
                     "JOIN parameter p on pma.ID_parameter = p.ID_parameter " +
                     $"WHERE material.type = \"{materialName}\" and pma.type = \"Коэффициент\"";
         using var connection = JoinBase();
         var command = new MySqlCommand();
         command.Connection = connection;
         command.CommandText = query;
         var reader = command.ExecuteReader();
         var result = new List<MaterialInfo>();
         while (reader.Read()) {
             var first = reader.GetString(0);
             var second = reader.GetDouble(1);
             var isNull = reader.IsDBNull(2);
             var third = isNull ? "" : reader.GetString(2);
             result.Add(new (first, second, third));
         }
         connection.Close();
    
         return result;
     }
    //
    // public static Dictionary<string, string> GetMaterialsValues(string selectedMaterial) {
    //
    //     const string query = "select name, value " +
    //                          "from material " +
    //                          "join parameter_material_attr pma on pma.ID_material = material.ID_material " +
    //                          "and pma.type = \"Свойство материала\" " +
    //                          "and material.type = @material " +
    //                          "join parameter p on p.ID_parameter = pma.ID_parameter";
    //     
    //     using var connection = JoinBase();
    //     var command = new MySqlCommand();
    //     command.Connection = connection;
    //     command.Parameters.AddWithValue("@material", selectedMaterial);
    //     command.CommandText = query;
    //     var reader = command.ExecuteReader();
    //     var result = new Dictionary<string, string>();
    //     while (reader.Read()) {
    //         result.Add(reader.GetString(0), reader.GetString(1));
    //     }
    //     connection.Close();
    //     
    //     return result;
    // }
}