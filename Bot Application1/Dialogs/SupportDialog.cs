using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using System.Collections;

namespace Bot_Application1.Dialogs
{

    [Serializable]
  [LuisModel("792b7d7a-a4b8-488c-b193-9d846b3faaf4", "1c3967ddd46a49bfba05d800a40e6997")]
    public class SupportDialog : LuisDialog<object>
    {
       Hashtable hash = new Hashtable();
        bool locked;


         /* override public async Task StartAsync(IDialogContext context)
             {
           // await context.PostAsync("Hallooooo");
            context.Wait(MessageReceived);
             }*/
             
        /*public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("You said: " + message.Text);
            context.Wait(MessageReceivedAsync);
        }*/

        [LuisIntent("UserInfo")]
        public async Task userInfoIntent(IDialogContext context, LuisResult result)
        {
            EntityRecommendation firstName;
            EntityRecommendation lastname;
            EntityRecommendation userId;
            if (!hash.ContainsKey("firstname"))
            {
                if (!result.TryFindEntity("FirstName", out firstName))
                {
                    await context.PostAsync("Please tell me your firstname");

                    firstName = new EntityRecommendation();
                  
                }
                else
                {
                    hash.Add("firstname", firstName);
                  
                }
            }


             if (!hash.ContainsKey("lastname"))
            {
                if (!result.TryFindEntity("LastName", out lastname))
                {

                    await context.PostAsync("Please tell me your last name");
                    lastname = new EntityRecommendation();
                 
                }
                else
                {
                    hash.Add("lastname", lastname);
                }
            }

            if (!hash.ContainsKey("id") && hash.ContainsKey("firstname") && hash.ContainsKey("lastname"))
            {


                if (!result.TryFindEntity("UserId", out userId))
                {
                    await context.PostAsync("Please tell me your User-Id");
                    userId = new EntityRecommendation();
                  
                }
                else
                {
                    hash.Add("id", userId);

                }
            }

            if(hash.ContainsKey("firstname") && hash.ContainsKey("lastname") && hash.ContainsKey("id"))
            {
                await context.PostAsync("Do you need any help, "+ ((EntityRecommendation)hash["firstname"]).Entity.ToString()+" "+ ((EntityRecommendation)hash["lastname"]).Entity.ToString()+"? Is it with your Password, Outlook or somthing different?");
                
            }

            context.Wait(MessageReceived);

        }


        [LuisIntent("PasswordReset")]
        public async Task Password(IDialogContext context, LuisResult result)
        {
            EntityRecommendation forgot;
            if (!hash.ContainsKey("firstname") || !hash.ContainsKey("lastname") || !hash.ContainsKey("id"))
            {
                await context.PostAsync("Please tell me your name");
                context.Wait(MessageReceived);
            }
            else
            {
                if (result.TryFindEntity("forgot", out forgot))
                {
                    await context.PostAsync("Ok, we have created a ticket. Someone will contact you soon.");

                    hash.Clear();
                }

                else if (result.TryFindEntity("locked", out forgot))
                {
                    locked = true;
                    await context.PostAsync("Do you remember your password?");
                    context.Wait(MessageReceived);
                    
                }
                else if(!locked)
                {
                    await context.PostAsync("Did you forgot your passwort or locked your account?");
                    context.Wait(MessageReceived);
                }

                if (locked)
                {
                    if (result.TryFindEntity("yes", out forgot))
                    {
                        await context.PostAsync("Ok, I unlocked your account.");
                        locked = false;
                        hash.Clear();
                    }
                    else if (result.TryFindEntity("no", out forgot))
                    {
                        await context.PostAsync("Ok, we have created a ticket. Someone will contact you soon.");
                        locked = false;
                        hash.Clear();
                    }
                }
                
                
            }

        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I didn't understand your message.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("Hello from the other sideee! Welcome to our customer support service. Can you please provide your full name and id?");
            context.Wait(MessageReceived);
        }







        /* public SupportDialog()
          {

          }


          public async Task UserInfo(IDialogContext context, LuisResult result)
          {
              EntityRecommendation title;
              if (result.TryFindEntity("Name",out title))
              {
                  await context.PostAsync("Your name is!"+title.ToString());

              }


          }*/


















    }
}