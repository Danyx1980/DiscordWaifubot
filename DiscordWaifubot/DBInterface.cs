using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace DiscordWaifubot
{
    class DBInterface
    {
        private static IMongoClient client = new MongoClient();
        private static IMongoDatabase waifuBotDatabase = client.GetDatabase("Events");
        
        private static string eventCollection = "Events";
        private static string REventCollection = "REvents";
        
        private static string logInCollection = "LogIn";
        private static string RLogInCollection = "RLogIn"; 
        
        private static string logOffCollection = "LogOff";
        
        private static string RUserCollection = "RUsers"; 
        private static string userCollection = "Users";

        private static string commandCollection = "Commands"; 

        public async void insertEvent(List<string> eventInfo)
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(REventCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("Command", eventInfo[0]);

            List<BsonDocument> tempList = await collection.Find(filter).ToListAsync();

            if (tempList.Count > 0)
            {
                if (eventInfo.Count == 3)
                {
                    var update = Builders<BsonDocument>.Update
                        .Set("Date", eventInfo[1])
                        .Set("Hour", eventInfo[2]);
                    var result = await collection.UpdateOneAsync(filter, update);
                }
                if (eventInfo.Count == 4)
                {
                    var update = Builders<BsonDocument>.Update
                        .Set("Date", eventInfo[1])
                        .Set("Hour", eventInfo[2])
                        .Set("Extra", eventInfo[3]);
                    var result = await collection.UpdateOneAsync(filter, update);
                }
            }
            else
            {
                if (eventInfo.Count == 4)
                {
                    var toInsert = new BsonDocument
                    {
                        {"Event", eventInfo[0]},
                        {"Date", eventInfo[1]},
                        {"Hour", eventInfo[2]},
                        {"Extra", eventInfo[3]}
                    };
                    await collection.InsertOneAsync(toInsert);
                }

                if (eventInfo.Count == 3)
                {
                    var toInsert = new BsonDocument
                    {
                        {"Event", eventInfo[0]},
                        {"Date", eventInfo[1]},
                        {"Hour", eventInfo[2]}
                    };
                    await collection.InsertOneAsync(toInsert);
                }
            }
        }

        public async Task<List<BsonDocument>> getEvent(string eventInfo)
        {
            string info = string.Concat(' ', eventInfo);

            var Rcollection = waifuBotDatabase.GetCollection<BsonDocument>(REventCollection);
            var Rfilter = Builders<BsonDocument>.Filter.Eq("Event", eventInfo);
            List<BsonDocument> RResult = await Rcollection.Find(Rfilter).ToListAsync();

            if (RResult.Count > 0)
                return RResult; 

            var collection = waifuBotDatabase.GetCollection<BsonDocument>(eventCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("Event", info);
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return result.ToList(); 
        }

        public async void insertCustomCommand(string command, string text)
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(commandCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("Command", command);

            List<BsonDocument> tempList = await collection.Find(filter).ToListAsync();

            if (tempList.Count > 0)
            {
                var update = Builders<BsonDocument>.Update
                    .Set("Text", text);

                var result = await collection.UpdateOneAsync(filter, update); 
            }
            else
            {
                var toInsert = new BsonDocument
                {
                    {"Command", command},
                    {"Text", text},
                };

                await collection.InsertOneAsync(toInsert);
            }
        }

        public async Task<List<BsonDocument>> getCustomCommand(string command)
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(commandCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("Command", command);
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return result;  
        }

        public async Task<List<BsonDocument>> getAllCommands()
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(commandCollection);
            var filter = new BsonDocument();
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return result;
        }

        public async void removeCommand (string command)
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(commandCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("Command", command);
            await collection.DeleteOneAsync(filter); 
        }

        public async void inserLogIn(List<string> LogInfo)
        {
            var toInsert = new BsonDocument
            {
                {"Date", LogInfo[0]},
                {"Hour", LogInfo[1]},
                {"IP", LogInfo[2]},
                {"User", LogInfo[3]},
                {"ID", LogInfo[3].ToLower()}
            };

            var collection = waifuBotDatabase.GetCollection<BsonDocument>(logInCollection);
            await collection.InsertOneAsync(toInsert);
        }

        public async Task<List<BsonDocument>> getLastSeen(string user)
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(logInCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("ID", user.ToLower());
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return result.ToList();
        }

        public async void inserLogOut(List<string> LogInfo)
        {
            var toInsert = new BsonDocument
            {
                {"Date", LogInfo[0]},
                {"Hour", LogInfo[1]},
                {"User", LogInfo[2]},
                {"ID", LogInfo[2].ToLower()}
            };

            var collection = waifuBotDatabase.GetCollection<BsonDocument>(logOffCollection);
            await collection.InsertOneAsync(toInsert);
        }

        public async Task<List<BsonDocument>> getLogOut(string user)
        {
            var collection = waifuBotDatabase.GetCollection<BsonDocument>(logOffCollection);
            var filter = Builders<BsonDocument>.Filter.Eq("ID", user.ToLower());
            List<BsonDocument> result = await collection.Find(filter).ToListAsync();

            return result.ToList();
        }
    }
}
