using Azure;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.ML.Data;
using p2p.Common.Models;
using p2p.Logic.Contract;
using p2p.Logic.Imp;
using System.Data;
using System.Data.SqlClient;
using System.Formats.Asn1;
using System.Transactions;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;
using static p2p.Common.Models.Users;



namespace p2p.Controllers
{
    //  [MyAuthorizationFilter]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;
        private readonly ILogger<SpaceController> _logger;

        public AdminController(IAdminManager adminManager,
            ILogger<SpaceController> logger)
        {
            _adminManager = adminManager;
            _logger = logger;
        }


        [HttpPost()]
        public IActionResult AllUserList(input_1 detail)
        {
            try
            {
                var users = _adminManager.AllUserList(detail);
                if (users != null && users.Count > 0)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound("No spacers found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult EnableDisable(ongoning_booking detail)
        {

            ResponseData response = _adminManager.EnableDisable(detail);
            try
            {
                if (response.IsSaved)
                {
                    return Ok(new { Message = response.Message });
                }
                else
                {
                    return BadRequest(new { Error = "Failed to update state." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }

        }


        [HttpPost()]
        public IActionResult ContactUs(contactus info)
        {
            try
            {
                ResponseData response = _adminManager.ContactUs(info);

                if (response.IsSaved)
                {
                    bool result = true;
                    string message = response.Message;
                    return Ok(new { result = result, message = message });
                }
                else
                {
                    bool nresult = false;
                    string message = response.Message;
                    return Ok(new { result = nresult, message = message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }




        [HttpPost()]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null || file.Length == 0) return BadRequest("Upload a file.");

                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                if (file.FileName.EndsWith(".csv"))
                {
                    // ProcessCsv(filePath);
                    return Ok(new { file.FileName, size = file.Length });
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    List<ValidationError> validationErrors = ProcessExcels(filePath);
                 
                    if (validationErrors.Count > 0)
                    {
                        return Ok(new { Errors = validationErrors });
                    }
                    else
                    {
                        SaveExcelData(filePath);
                        bool result = true;
                        string message ="File Uploaded";
                        return Ok(new { result = result, message = message });
                    }
                }
                else
                {
                    return BadRequest("Invalid file format.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = ex.Message });
            }
        }

        private List<ValidationError> ProcessExcels(string filePath)
        {
            List<ValidationError> validationErrors = new List<ValidationError>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var dataTable = result.Tables[0];
                    var spaces = new List<Space_Owner_Master>();

                    // Extract column names from the first row
                    var columnNames = new List<string>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        columnNames.Add(dataTable.Rows[0][column.ColumnName].ToString().Trim());
                    }

                    // Get column no
                    int usernameIndex = columnNames.IndexOf("Username");
                    int longitudeIndex = columnNames.IndexOf("Longitude");
                    int latitudeIndex = columnNames.IndexOf("Latitude");
                    int descriptionIndex = columnNames.IndexOf("Description");
                    int priceIndex = columnNames.IndexOf("Price");
                    int appartmentNoIndex = columnNames.IndexOf("Address_Appartment_no");
                    int streetIndex = columnNames.IndexOf("Address_street");
                    int districtIndex = columnNames.IndexOf("Address_District");
                    int spaceImagePathIndex = columnNames.IndexOf("Space_Image_Path");


                    // Start from the second row as the first row was used to extract column names
                    for (int i = 1; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];


                        for (int columnIndex = 0; columnIndex < columnNames.Count; columnIndex++)
                        {
                            string columnValue = row[columnIndex].ToString().Trim();

                            if (string.IsNullOrWhiteSpace(columnValue))
                            {

                                validationErrors.Add(new ValidationError
                                {
                                    ColumnName = columnNames[columnIndex],
                                    RowNumber = i + 1,
                                    ErrorMessage = $" cannot be empty or whitespace."
                                });
                            }

                            // Assuming longitude and latitude are obtained from the columnValue or any other source
                            double currentLongitude;
                            double currentlatitude;

                            // Parse longitude and latitude values
                            if (columnNames[columnIndex] == "Longitude")
                            {
                                if (!double.TryParse(columnValue, out currentLongitude) || !IsValidLongitude(currentLongitude))
                                {

                                    validationErrors.Add(new ValidationError
                                    {
                                        ColumnName = columnNames[columnIndex],
                                        RowNumber = i + 1,
                                        ErrorMessage = $"Invalid longitude value."
                                    });
                                }
                            }
                            else if (columnNames[columnIndex] == "Latitude")
                            {
                                if (!double.TryParse(columnValue, out currentlatitude) || !IsValidLongitude(currentlatitude))
                                {
                                    validationErrors.Add(new ValidationError
                                    {
                                        ColumnName = columnNames[columnIndex],
                                        RowNumber = i + 1,
                                        ErrorMessage = $"Invalid latitude value."
                                    });

                                }
                            }

                        }

                        // Perform validation for each column before adding to the spaces list
                        string username = usernameIndex != -1 ? row[usernameIndex].ToString().Trim() : "";
                        string longitude = longitudeIndex != -1 ? row[longitudeIndex].ToString().Trim() : "";
                        string latitude = latitudeIndex != -1 ? row[latitudeIndex].ToString().Trim() : "";
                        string description = descriptionIndex != -1 ? row[descriptionIndex].ToString().Trim() : "";
                        decimal price = priceIndex != -1 && decimal.TryParse(row[priceIndex].ToString(), out decimal parsedPrice) ? parsedPrice : 0;
                        string appartmentNo = appartmentNoIndex != -1 ? row[appartmentNoIndex].ToString().Trim() : "";
                        string street = streetIndex != -1 ? row[streetIndex].ToString().Trim() : "";
                        string district = districtIndex != -1 ? row[districtIndex].ToString().Trim() : "";
                        string spaceImagePath = spaceImagePathIndex != -1 ? row[spaceImagePathIndex].ToString().Trim() : "";

                   
                   
                    }
                    return validationErrors;

                }
            }
        }

        bool IsValidLongitude(double longitude)
        {
            string longitudeString = longitude.ToString();
            if (!double.TryParse(longitudeString, out double parsedLongitude))
            {
                return false; // Not a valid numerical format
            }
            return parsedLongitude >= -180 && parsedLongitude <= 180;
        }

        private void SaveExcelData(string filePath)
        {
            // Load Excel file using ExcelDataReader
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Treat first row as column names
                        }
                    });

                    DataTable dataTable = dataSet.Tables[0]; // Assuming there's only one table in the Excel file

                    // Convert DataTable to a list of Space_Owner_Master objects
                    List<Space_Owner_Master> records = new List<Space_Owner_Master>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Assuming column order in Excel matches the order of properties in Space_Owner_Master
                        Space_Owner_Master space = new Space_Owner_Master
                        {
                            RoleID = 2,
                            Username = row["Username"].ToString(),
                            Longitude = row["Longitude"].ToString(),
                            Latitude = row["Latitude"].ToString(),
                            Description = row["Description"].ToString(),
                            Price =  decimal.TryParse(row["Price"].ToString(), out decimal parsedPrice) ? parsedPrice : 0,
                            Address_Appartment_no = row["Address_Appartment_no"].ToString(),
                            Address_street = row["Address_street"].ToString(),
                            Address_District = row["Address_District"].ToString(),
                            Space_Image_Path = row["Space_Image_Path"].ToString(),
                            Created_By = row["Username"].ToString(),
                            Created_Date = DateTime.Now,
                            Enable = true 
                        };

                        records.Add(space);
                    }
                    SaveData(records);
                }
            }
        }

        private void SaveData(List<Space_Owner_Master> records)
        {

            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                //    using (var transaction = connection.BeginTransaction())
                //    {
                try
                {
                    foreach (var space in records)
                    {
                        bool similarRecordExists = SimilarRecordExists(connection, space);

                        if (!similarRecordExists)
                        {

                            string databaseName = "p2p";
                            string query = @$"USE {databaseName}; 
                                           INSERT INTO Space_Owner_Master (RoleID,Username,Longitude,Latitude,Description,Price,Address_Appartment_no,Address_street,Address_District,Space_Image_Path,Created_By,Created_Date,Modified_By,Modified_Date,Enable)
                                           VALUES (2,'{space.Username}','{space.Longitude}','{space.Latitude}','{space.Description}','{space.Price}','{space.Address_Appartment_no}','{space.Address_street}','{space.Address_District}','{space.Space_Image_Path}','{space.Username}','{DateTime.Now}','','',1)";

                            using (var command = new SqlCommand(query, connection))//, transaction
                            {
                                command.ExecuteNonQuery();
                            }

                        }
                    }
                    //     transaction.Commit();
                }
                catch (Exception ex)
                {
                    //      transaction.Rollback();
                    throw ex;
                }
                // }
            }
        }
      
        private bool SimilarRecordExists(SqlConnection connection, Space_Owner_Master space)
        {
            string query = @$"SELECT COUNT(*) FROM Space_Owner_Master WHERE 
                              Username = '{space.Username}'AND 
                              Longitude = '{space.Longitude}' AND 
                              Latitude = '{space.Latitude}' AND 
                              Description = '{space.Description}' AND                               
                              Price = '{space.Price}'AND 
                              Address_Appartment_no = '{space.Address_Appartment_no}' AND 
                              Address_street = '{space.Address_street}' AND 
                              Address_District = '{space.Address_District}' AND 
                              Space_Image_Path = '{space.Space_Image_Path}'";

            using (var command = new SqlCommand(query, connection))
            {
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    
    
    
    
    
    }
}
