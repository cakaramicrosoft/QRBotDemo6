using System;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    public const string Entity_Flight_Code = "flightcode";
    public const string Entity_Flight_Date = "builtin.datetime.date";
    public const string Entity_Flight_Date_Arrival = "isDateArrival";
    public const string Entity_City = "builtin.geography.city";


    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
    }

    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($" Again Congrats! You have reached the none intent. You said: {result.Query}"); //
        context.Wait(MessageReceived);
    }

    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    [LuisIntent("MyIntent")]
    public async Task MyIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($"You have reached the MyIntent intent. You said: {result.Query}"); //
        context.Wait(MessageReceived);
    }

    [LuisIntent("greeting")]
    public async Task GreetIntent(IDialogContext context, LuisResult result)
    {
        await context.PostAsync($" Hi! How can I help you today?"); //
        context.Wait(MessageReceived);
    }

    [LuisIntent("flightstatus")]
    public async Task FlightStatusIntent(IDialogContext context, LuisResult result)
    {
        string flight_code = "";
        string flight_date = "";
        string flightDateArrivalorDeparture = "Departure";
        bool allChecksPassed = true;
        string flight_from_city = "";
        EntityRecommendation title;
        //Find if the customer specified the flight code:
        if (result.TryFindEntity(Entity_Flight_Code, out title))
        {
            flight_code = title.Entity;
            //context.Wait(MessageReceived);
        }
        else
        {
            await context.PostAsync($"You didn't specift a Flight Code!");
            allChecksPassed = false;

        }
        //Find the from city:
        if (result.TryFindEntity(Entity_City, out title))
        {
            flight_from_city = title.Entity;

        }

        // Find if the customer specified the date is arrival:

        if (result.TryFindEntity(Entity_Flight_Date_Arrival, out title))
        {
            flightDateArrivalorDeparture = "Arrival";
        }

        // Find if the customer specified the flight date:
        if (result.TryFindEntity(Entity_Flight_Date, out title))
        {
            flight_date = title.Entity;
        }
        else
        {
            flight_date = "today";
        }
        if (allChecksPassed)
        {
            await context.PostAsync($"You asked for Flight Status for Flight:" + flight_code + " " + flightDateArrivalorDeparture + " " + flight_date + " from city:" + flight_from_city);
        }

        context.Wait(MessageReceived);
    }
}
}