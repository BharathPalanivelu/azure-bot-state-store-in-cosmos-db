using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot_State_Application.Dialogs
{
    [Serializable]
    public class StateDialog: IDialog<object>
    {
      


        public async Task StartAsync(IDialogContext dialogContext)
        {
            await dialogContext.PostAsync("Provide some details to help you");
            dialogContext.Wait(ConversationStart);
        }

        public async Task ConversationStart(IDialogContext dialogContext, IAwaitable<IMessageActivity> result)
        {
            var patiantName = await result;
            //Set user  data
            await dialogContext.PostAsync("Please provide patient name");
            dialogContext.Wait(GetPatientName);
        }

        public async Task GetPatientName(IDialogContext dialogContext, IAwaitable<IMessageActivity> result)
        {
            var patiantName = await result;
            //Set conversion  data
            dialogContext.UserData.SetValue("patientname", patiantName.Text);
            await dialogContext.PostAsync("Please provide patient address");
            dialogContext.Wait(GetPatientAddress);
        }
        public async Task GetPatientAddress(IDialogContext dialogContext, IAwaitable<IMessageActivity> result)
        {
            var patiantAddress = await result;
            //Set conversion  data
            dialogContext.ConversationData.SetValue("address", patiantAddress.Text);
            await dialogContext.PostAsync("Please provide patient medicalHistory");
            dialogContext.Wait(GetPatientMedicalHistory);
        }

        public async Task GetPatientMedicalHistory(IDialogContext dialogContext, IAwaitable<IMessageActivity> result)
        {
            var medicalHistory = await result;
            //Set PrivateConversationData  data
            dialogContext.PrivateConversationData.SetValue("patientMedicalHistory", medicalHistory.Text);
            await dialogContext.PostAsync("Do you want to see all patient info");
            dialogContext.Wait(ShowPatientData);
        }

        public async Task ShowPatientData(IDialogContext dialogContext, IAwaitable<IMessageActivity> result)
        {
            var yes = await result;
            //get data
            //get userdata
            var name = dialogContext.UserData.GetValue<string>("patientname");
            var address = dialogContext.ConversationData.GetValue<string>("address");
            var medicalHistory = dialogContext.PrivateConversationData.GetValue<string>("patientMedicalHistory");
            var data = $"Details retrieved from Bot state  >> Patient Name: {name}, Address: {address}, MedicalHistory: {medicalHistory} ";
            await dialogContext.PostAsync(data);
            dialogContext.Wait(ConversationStart);
        }

    }
}