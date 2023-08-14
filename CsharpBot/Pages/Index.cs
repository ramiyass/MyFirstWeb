using CsharpBot.Model;
using CsharpBot.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Runtime.Serialization.Formatters;

namespace CsharpBot.Pages
{
    public partial class Index
    {
        private  string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory =new List<Message>();
        bool isSendingMessage;
        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key is not "Enter") return;
            await SendMessage();
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(_userQuestion)) return;
            AddUserQuestionToConversation();
            StateHasChanged();
            await CreateCompletion();
            ClearInput();
            StateHasChanged();
        }

        private void ClearInput()
       =>_userQuestion = string.Empty;

        private async Task CreateCompletion()
        {
            isSendingMessage=true;
            var assistantResponse= await OpenAIService.CreateChatCompletion(_conversationHistory);
            _conversationHistory.Add(assistantResponse);
            isSendingMessage=false;
        }

        private void AddUserQuestionToConversation()
        =>_conversationHistory.Add(new Message {role="user",content=_userQuestion});
        private void ClearConservation()
        {
            ClearInput();
            _conversationHistory.Clear();
        }
        [Inject]
        public OpenAIService OpenAIService { get; set; }
        public List<Message> Messages => _conversationHistory.Where(c=>c.role is not "system ").ToList();
        
    }
}
