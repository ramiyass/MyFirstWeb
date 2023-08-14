﻿using CsharpBot.Model;
using Microsoft.Extensions.Options;

namespace CsharpBot.Services
{
   
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ChatOptions> _options;
        public OpenAIService(HttpClient httpClient, IOptions<ChatOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }
        public async Task<Message> CreateChatCompletion(List<Message> messages)
        {
            var request= new { Model=_options.Value.GtpModel,messages=messages.ToArray()};
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.Value.ApiKey);
            var response= await _httpClient.PostAsJsonAsync(_options.Value.ApiUrl, request);    
            response.EnsureSuccessStatusCode();
            var chatCompletionResponse= await  response.Content.ReadFromJsonAsync<ChatbotResponse>();
            return chatCompletionResponse?.choices.First().message;
        }
    }
}
