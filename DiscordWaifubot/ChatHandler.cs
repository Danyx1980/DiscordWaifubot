using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DiscordWaifubot
{
    class ChatHandler
    {
        private string nickname;
        private string input;
        private string[] splitInput; 
        private DBInterface dbinterface = new DBInterface(); 

        public string GetResponse (string user, string message)
        {
            List<string> response = new List<string>();
            splitInput = message.Split(new Char[] { ' ' });
            input = message;
            nickname = user; 

            string[] website = { ".com", ".moe", ".org", ".net", "http" };
            string[] possibleHate = { "fuck you", "i hate you", "you suck", "youre useless", "you're useless", "you useless", "nobody wants you", "get out", "fuck off" };
            string[] possibleSalutes = { "hello", "kombawa", "ohayo", "good morning", "morning", "hey", "ohayou" };
            string[] possibleLove = { "i love you", "you're so cool", "you so cool", "youre so cool", "you're awesome", "you awesome", "youre awesome", "is cool", "is cute", "is pretty", "are cute", "are pretty", "you're pretty", "you're so pretty", "youre pretty", "youre so pretty", "youre cute", "youre so cute", "you are so pretty", "you are so cute", "you are so cool", "i love", "is amazing", "is awesome", "you are awesome" };

            foreach (string substring in splitInput)
            {
                if (substring.ToLower().Contains("hentai"))
                {
                    foreach (string ignore in website)
                    {
                        if (substring.ToLower().Contains(ignore)) return null;
                    }
                    return Hentai();
                }
            }

            foreach (string hate in possibleHate)
                if (input.ToLower().Contains(hate) && input.ToLower().Contains(WaifuBot.NICK.ToLower())) return Sad();

            foreach (string salutes in possibleSalutes)
                if (input.ToLower().Contains(salutes) && input.ToLower().Contains(WaifuBot.NICK.ToLower())) return Domo();

            if (splitInput[0].ToLower() == "domo" && splitInput.Length == 1) return Domo();

            foreach (string love in possibleLove)
                if (input.ToLower().Contains(love) && input.ToLower().Contains(WaifuBot.NICK.ToLower())) return Love();

            if (input.ToLower().Contains("!calm"))
            {
                string substring = input.Substring(input.IndexOf("!calm"));
                try
                {
                    string[] substringsplit = substring.Split(new Char[] { ' ' });
                    if (substringsplit.Length > 1) nickname = substringsplit[1];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return Calm();
            }

            if (splitInput[0].ToLower().StartsWith("ayy") && splitInput.Length == 1) return Lmao();

            if (splitInput[0].ToLower() == "!schedule") return Schedule();

            if (splitInput[0].ToLower() == "!event") return Event();

            /*if (splitInput[3].ToLower().Contains("!lastseen")) return LastSeen();

            if (splitInput[3].ToLower().Contains("!fullinfo"))
            {
                sender = nickname;
                return FullInfo();
            }*/

            if (input.ToLower().Contains("!rules")) return Rules();

            if (splitInput[0].ToLower() == "!addcommand") return AddCommand(); 

            if (splitInput[0].ToLower().StartsWith("!!")) return Command();

            if (splitInput[0].ToLower() == "!allcommands" && splitInput.Length == 1) return AllCommands();

            if (splitInput[0].ToLower() == "!removecommand") return RemoveCommand(); 

            else return null;
        }

        private string Hentai()
        {
            string[] hentaiResponse = { "L-lewd!", "Uh- UUHH?!", "D-darin, you like those stuff?", "W-What are you talking about?!", "Darin! I thought you were better than this!", "D-do you really like that? Am I not enough?", "Please! Let's keep the chat clean!" };
            Random answer = new Random();

            return hentaiResponse[answer.Next(0, hentaiResponse.Length)];
        }

        private string Domo()
        {
            string[] domoResponse = { "Ara? Ohayou darin ", "Good morning ", "Hmmm! I don't like being called that! you know? " };
            Random answer = new Random();

            if (nickname.ToLower().Contains("cult_films") && answer.Next(0, 20) == 13) return "Uh? Don't get near me, you creep";
            if (nickname.ToLower().Contains("anpan")) return (domoResponse[answer.Next(0, domoResponse.Length - 1)] + nickname);
            return (domoResponse[answer.Next(0, domoResponse.Length - 1)] + nickname + "-san");
        }

        private string Sad()
        {
            string[] hateResponses = { "Wow so mean, ", "*cries* \r\n Why so mean ", "I hve feelings too, you know? " };
            Random answer = new Random();

            if (nickname.ToLower().Contains("mahdi") && answer.Next(0, 10) == 5) return "No need to be so mean, Mahdi-san";
            else if (nickname.Contains("Rumi") && answer.Next(0, 10) == 3) return "Wow Rumi-chan, you're so mean :(";
            else if (answer.Next(0, 999) == 337) return ("It's ok, I still love you " + nickname + "-san");

            return (hateResponses[answer.Next(0, hateResponses.Length - 1)] + nickname + "-san");

        }

        private string Love()
        {
            string[] loveResponses = { "*blushes*", "Ara ara, you're making me blush!", "Darin, don't just say stuff like that!", "Oh darin!", "Oh darin! \r\n * blushes*" };
            Random answer = new Random();

            if (nickname.ToLower().Contains("naticus") && answer.Next(0, 10) == 7) return "Awww, stop it Lona-san!";
            if (nickname.ToLower().Contains("cult_films") && answer.Next(0, 50) == 34) return "Ew! get away from me, you freak!";
            if (answer.Next(0, 300) == 264) return "I already knew that, but thanks";

            return loveResponses[answer.Next(0, loveResponses.Length - 1)];

        }

        private string Calm()
        {
            string[] calmResponses = { "Ara ara, no need to be mad", "C'mon darin, sit here and relax", "Ara? Why are you so mad darin-kun?", "*headpats* \r\n Ara ara" };
            Random answer = new Random();

            if (nickname.ToLower().Contains("rumi") && answer.Next(0, 7) == 6) return "C'mon Rumi-chan, no need to be angry now";
            if (nickname.ToLower().Contains("mahdi") && answer.Next(0, 7) == 5) return "Now, now, Mahdi-san, don't let the salt get onto you";

            return calmResponses[answer.Next(0, calmResponses.Length - 1)];
        }

        private string Lmao()
        {
            string[] lmaoResponses = { "lmao", "papi" };
            string lmao = "lm";
            string papi = "pap"; 
            Random answer = new Random();

            if (answer.Next(0, 99) < 75)
            {
                if (input.Length - input.ToLower().Replace("y", "").Length > 2)
                {
                    for (int i = 0; i < input.Length - 2; i++)
                    {
                        lmao = string.Concat(lmao, 'a');
                    }
                    lmao = string.Concat(lmao, 'o');
                    return lmao;
                }
            }
            else
            {
                if (input.Length - input.ToLower().Replace("y", "").Length > 2)
                {
                    for (int i = 0; i < input.Length - 2; i++)
                    {
                        papi = string.Concat(papi, 'i');
                    }
                    return papi;
                }
            }
            if (answer.Next(0, 99) < 75)
                return lmaoResponses[0];
            else
                return lmaoResponses[1]; 
        }

        private string Rules()
        {
            string[] rules = { "Post dongers.\n1. Praise Camilla. \n2. Remember Anna (Miyu Matsuki Sept. 14, 1977 - Oct. 27, 2015). \n3. Love Saki. \n4. Never talk about 9/7." };
            return rules[0];
        }

        private string error()
        {
            string[] error = { "Ara? Something doesn't seem to be right... Try again later.", "Something's wrong honey, try again later, ok?", "That didn't seem to work, can you try again later?", "Darin, that's weird, that didn't seem to work. Canyou try again later?" };
            Random answer = new Random();

            return error[answer.Next(0, error.Length - 1)];
        }

        private string Schedule()
        {
            string[] confirmation = { "Got it!", "Okay honey, I'll save that.", "Done!", "Here you go darin" };
            List<string> eventInfo = new List<string>(); 
            Random answer = new Random();

            string commandMessage = input.Substring(input.ToLower().IndexOf("!schedule") + 10);
            string[] splitCommandMessage = commandMessage.Split(new string[] { " , " }, StringSplitOptions.RemoveEmptyEntries);
            splitCommandMessage[0].Replace(" ", ""); 
            foreach (string command in splitCommandMessage) eventInfo.Add(command); 

            try
            {
                dbinterface.insertEvent(eventInfo);
                if (eventInfo.Count == 4)
                    return confirmation[answer.Next(0, confirmation.Length - 1)] + string.Format("\n{0} is scheduled for {1} at {2}. An additional note is attached, it says: {3}", eventInfo[0], eventInfo[1], eventInfo[2], eventInfo[3]);
                else
                    return confirmation[answer.Next(0, confirmation.Length - 1)] + string.Format("\n{0} is now scheduled for {1} at {2}. ", eventInfo[0], eventInfo[1], eventInfo[2]);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return error();
            }

        }

        private string Event()
        {
            try
            {
                List<BsonDocument> eventInfo = dbinterface.getEvent(splitInput[1]).Result;

                if (eventInfo.Last().Count() == 4)
                    return string.Format("{0} is scheduled for {1} at {2}.\n", eventInfo.Last()[1], eventInfo.Last()[2], eventInfo.Last()[3]);

                else
                    return string.Format("{0} is scheduled for {1} at {2}. An additional note is attached, it says: {3}.\n", eventInfo.Last()[1], eventInfo.Last()[2], eventInfo.Last()[3], eventInfo.Last()[4]);

            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return error();
            }

        }

        private string AddCommand()
        {
            string[] confirmation = { "Got it!", "Okay honey, I'll save that.", "Done!", "Here you go darin" };
            string text = input.Substring(input.IndexOf(splitInput[2])); 
            Random answer = new Random(); 

            dbinterface.insertCustomCommand(splitInput[1], text);

            return confirmation[answer.Next(0, confirmation.Length - 1)] + string.Format("\n'{0}' is now a Custom Command for '{1}'. Remember to use '!!' to call it!", splitInput[1], text); 
        }

        private string Command()
        {
            string command = splitInput[0].Substring(2); 
            try
            {
                List<BsonDocument> result = dbinterface.getCustomCommand(command).Result;

                return result.Last()[2].ToString(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); 
                return error(); 
            }
        }

        private string AllCommands()
        {
            List<BsonDocument> commands = dbinterface.getAllCommands().Result;
            string answer = "";

            foreach (BsonDocument command in commands)
            {
                answer = string.Concat(answer, command[1] + ", "); 
            }

            answer = answer.Remove(answer.Length - 2); 
            return answer; 
        }

        private string RemoveCommand()
        {
            string[] confirmation = { "Got it, sweetie", "Okay honey, I'll remove that.", "Done!", "Here you go darin" };
            Random answer = new Random();

            try
            {
                dbinterface.removeCommand(splitInput[1]);

                return confirmation[answer.Next(0, confirmation.Length - 1)] + string.Format("\n'{0}' has been removed as a command.", splitInput[1]);
            }
            catch (Exception e)
            {
                return error();
            }
        }
    }
}
