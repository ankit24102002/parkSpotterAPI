// ParkingPredictionService.cs
using Microsoft.ML;
using Microsoft.ML.Data;
using Org.BouncyCastle.Ocsp;
using static p2p.Common.Models.Space;

namespace p2p
{
    public class ParkingPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _trainedModel;


        public class ParkingData
        {
            [LoadColumn(0)] public float Hour;
            [LoadColumn(1)] public float DayOfWeek;
            [LoadColumn(2)] public float Demand;
            [LoadColumn(3)] public float Price; // Target variable
        }

        public class ParkingPrediction
        {
            [ColumnName("Score")]
            public float Price;
        }
        public ParkingPredictionService()
        {

            string dataPath = @"D:\Desktop\dataset.csv";
            _mlContext = new MLContext();
            var dataView = _mlContext.Data.LoadFromTextFile<ParkingData>(dataPath, hasHeader: true, separatorChar: ',');

            // Split data into training and test sets
            var dataSplit = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainingData = dataSplit.TrainSet;
            var testData = dataSplit.TestSet;

            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = _mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "DayOfWeekEncoded", inputColumnName: nameof(ParkingData.DayOfWeek))
                .Append(_mlContext.Transforms.Concatenate("Features", "Hour", "DayOfWeekEncoded", "Demand"));// 

            // Choose an algorithm and add to the pipeline
            var trainer = _mlContext.Regression.Trainers.FastTree(labelColumnName: "Price", featureColumnName: "Features");
            var trainingPipeline = dataProcessPipeline.Append(trainer);

             _trainedModel = trainingPipeline.Fit(dataView);

            // Evaluate the model
            var predictions = _trainedModel.Transform(testData);
            var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Price");

            // Output metrics
            Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError}");
            Console.WriteLine($"MAE: {metrics.MeanAbsoluteError}");

        }

        public float PredictPrice(float hour, float dayOfWeek, float demand)
        {
            var predictionFunction = _mlContext.Model.CreatePredictionEngine<ParkingData, ParkingPrediction>(_trainedModel);
            var sampleParking = new ParkingData { Hour = hour, DayOfWeek = dayOfWeek, Demand = demand };
            var prediction = predictionFunction.Predict(sampleParking);
            Console.WriteLine($"Predicted Price: {prediction.Price}");
          //  Console.ReadLine(); 

            return prediction.Price;
        }
    }
}
