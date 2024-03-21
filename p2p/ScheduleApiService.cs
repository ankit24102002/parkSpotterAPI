using System.Data.SqlClient;
using static p2p.Common.Models.Space;

namespace p2p
{
    public class ScheduleApiService : BackgroundService
    {
        private readonly ILogger<ScheduleApiService> _logger;
       // private readonly TimeSpan interval = TimeSpan.FromHours(24);
        
       private readonly TimeSpan interval = TimeSpan.FromMinutes(1);

        public ScheduleApiService(ILogger<ScheduleApiService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Scheduled task running at: {time}", DateTimeOffset.Now);

                await ApiLogic();

                // wait for the specific interval
                await Task.Delay(interval, stoppingToken);
            }
        }

        private async Task ApiLogic()
        {
           
            _logger.LogInformation("Executing API logic...");
            string connString = "server=ANKIT; database=userdatabase; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                ResponseData response = new ResponseData() { IsSaved = false, Message = "" };

                connection.Open();
                string databaseName = "p2p";
                string sql = $@"USE {databaseName};  UPDATE BookingDetail
                                                     SET Enable = 0
                                                     WHERE EndBooking <  GETUTCDATE() AND enable = 1;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.IsSaved = true;

                    }
                 
                }
            }



        }
    }
}
