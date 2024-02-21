using Godot;
using OpenAI_API;
using OpenAI_API.Models;
using OpenAI_API.Chat;
using System.Threading.Tasks;
public partial class ChatGD : Node
{
	OpenAIAPI api = new OpenAIAPI("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");

	Conversation chat;

	public override void _Ready()
	{

		chat = api.Chat.CreateConversation();

		chat.Model = Model.ChatGPTTurbo;
		chat.RequestParameters.Temperature = 0;

		//TestMessage();
	}

	public void SetContext(string context)
	{
		chat.AppendSystemMessage(context);
	}


	public async Task<string> SendMessage(string input)
	{

		chat.AppendUserInput(input);
		string response = await chat.GetResponseFromChatbotAsync();
		return response;

	}


}