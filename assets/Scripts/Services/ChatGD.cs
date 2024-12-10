using OpenAI_API;
using OpenAI_API.Models;
using OpenAI_API.Chat;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Godot;
using System.Collections.Generic;

public partial class ChatGD : Node
{
	OpenAIAPI api;

	SaveData saveData;
	public override async void _Ready()
	{
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		api = new OpenAIAPI(saveData.GetAPIKey("ChatGPT"));
		await GetModles();
	}
	#region ChatGPT
	public Conversation CreateConversation(Character character)
	{
		var chat = api.Chat.CreateConversation();
		chat.Model = Model.ChatGPTTurbo;
		chat.RequestParameters.Temperature = 1;
		GD.Print(character.context);
		chat.AppendSystemMessage(character.context);
		return chat;

	}

	public async Task<string> SendMessage(string input, Conversation chat)
	{
		try
		{
			GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Generating Response!", "[center]Chat GPT is now generating the response", 3);
			chat.AppendUserInput(input);
			GD.Print("past here");
			string response = await chat.GetResponseFromChatbotAsync();
			return response;
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("invalid_api_key"))
			{
				GD.PrintErr("Invalid API key. Please check your API key and try again.");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", $"[center]ChatGPT ERROR", $"[center]Invalid API key. Please check your API key and try again.", 10);
				return null;
			}
			else if (ex.Message.Contains("insufficient_quota"))
			{
				GD.PrintErr("Rate limit exceeded. Please wait and try again later.");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", $"[center]ChatGPT ERROR", $"[center]Rate limit exceeded. Please wait and try again later.", 10);
				return null;
			}
			else
			{
				GD.PrintErr($"General error: {ex.Message}");
				GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", $"[center]ChatGPT ERROR", $"[center]An error occurred. Please try again.", 10);
				return null;
			}
		}

	}

	public async Task<List<Model>> GetModles()
	{
		List<Model> models = await api.Models.GetModelsAsync();
		foreach (Model modle in models)
		{
			//GD.Print(modle.ModelID);
		}
		//GD.Print(models[0].ModelID);
		return models;
	}

	#endregion


}
