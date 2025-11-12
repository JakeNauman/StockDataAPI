StockDataAPI

Jake Nauman  11/12/2025
This is an API created in ASP.NET Core. 
It has 1 endpoint that takes in a stock name and returns the:
  Date (day)
  Average High
  Average Low
  Total Volume
of each day, for the past month (30 days). Data is colelcted across 15 minute intervals.

To run this API application:
  >> git clone https://github.com/JakeNauman/StockDataAPI
  
  In the solution folder, configure user secrets to the external api key you can obtain here: https://www.alphavantage.co/support/#api-key
  >> dotnet user-secrets init
>  > 
  >> dotnet user-secrets set "AlphaVantage:ApiKey" "YOUR_API_KEY"
  
  You should now be able to run the application and access the endpoint localhost:####/StockData/{StockName}

  
