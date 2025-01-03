using OpenAI;
using OpenAI.Chat;
using System;
using System.Threading.Tasks;
using Godot;
using System.Collections.Generic;
using NAudio.Midi;
using OpenAI.Models;

public partial class ChatGD : Node
{
	ChatClient client;
	Manager manager;
	SaveData saveData;

	ChatCompletionOptions options;

	public float temperature = 1f;
	public int maxTokenCount = 500;
	public string model = "gpt-3.5-turbo";
	public override void _Ready()
	{
		manager = GetNode<Manager>("/root/Managers");
		saveData = GetNode<SaveData>("/root/Data/SaveData");
		CreateNewClient();
	}

	#region ChatGPT

	public void CreateNewClient()
	{
		client = new(model: model, apiKey: saveData.GetAPIKey("ChatGPT"));
		options = new()
		{
			MaxOutputTokenCount = maxTokenCount,
			Temperature = temperature,
		};
		GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("info", "[center]Update GPT", "[center]successfully applied changes to Chatgpt", 3);
	}


	public List<ChatMessage> CreateConversation(Character character)
	{
		List<ChatMessage> messages = new()
		{
			new SystemChatMessage(character.context)
		};
		GD.Print(character.context);
		// Return the chat completion options object
		return messages;
	}

	public List<ChatMessage> CreateConversation(string context)
	{
		List<ChatMessage> messages = new()
		{
			new SystemChatMessage(context)
		};
		GD.Print(context);
		return messages;
	}

	public async Task<string> SendMessage(string input, List<ChatMessage> conversation)
	{
		try
		{
			GetNode<NotificationsManager>("/root/Managers/Notification")
				.NewNotification("info", "[center]Generating Response!", "[center]Chat GPT is now generating the response", 3);

			conversation.Add(new UserChatMessage(input));
			GD.Print("Sending input to ChatGPT...");
			ChatCompletion completion = await client.CompleteChatAsync(conversation, options);
			// Await the response from the chatbot
			conversation.Add(new SystemChatMessage(completion.Content.ToString()));
			return completion.Content[0].Text;
		}
		catch (Exception ex)
		{
			HandleException(ex);
			return null;
		}
	}

	public void UpdateChatHistory(Character character, string text)
	{
		foreach (Character activeCharacter in manager.character.ActiveCharacters)
		{
			if (character != activeCharacter)
			{
				activeCharacter.openChat.Add(new UserChatMessage($"[{character.name}] {text}"));
			}
		}
	}

	public void UpdateChatHistory(string name, string text)
	{
		foreach (Character activeCharacter in manager.character.ActiveCharacters)
		{

			activeCharacter.openChat.Add(new UserChatMessage($"[{name}] {text}"));

		}
	}

	private void HandleException(Exception ex)
	{
		string errorMessage = ex.Message.Contains("invalid_api_key") ?
			"Invalid API key. Please check your API key and try again." :
			ex.Message.Contains("insufficient_quota") ?
			"Rate limit exceeded. Please wait and try again later." :
			"An error occurred. Please try again.";

		GD.PrintErr($"Error: {ex.Message}");
		GetNode<NotificationsManager>("/root/Managers/Notification")
			.NewNotification("error", "[center]ChatGPT ERROR", $"[center]{errorMessage}", 10);
	}


	#endregion
}
