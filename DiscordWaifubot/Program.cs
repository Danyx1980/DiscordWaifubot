using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.API; 

namespace DiscordWaifubot
{
    class WaifuBot
    {
        public static string NICK = "WaifuBot"; 
        public static string EMAIL = "censored for you, baby";
        public static string PASSWORD = "( ͡° ͜ʖ ͡°)";

        static void Main(string[] args)
        {
            var client = new DiscordClient();
            ChatHandler chatHandler = new ChatHandler(); 

            client.LogMessage += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            //Echo back any message received, provided it didn't come from the bot itself
            client.MessageReceived += async (s, e) =>
            {
                if (!e.Message.IsAuthor)
                {
                    Console.WriteLine(e.Message.Text);
                    try
                    {
                        if (!e.Message.Channel.Name.Equals("nsfw") && !e.Message.Channel.Name.Equals("serious"))
                            await client.SendMessage(e.Channel, chatHandler.GetResponse(e.Message.User.Name, e.Message.Text));
                    }
                    catch { }
                }
            };

            client.Run(async () =>
            {
                await client.Connect(EMAIL, PASSWORD);
            });

            foreach (Channel channel in client.PrivateChannels)
                Console.WriteLine(channel.Name);

            Console.ReadLine();
        }
    }
}
