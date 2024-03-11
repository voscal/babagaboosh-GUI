using Godot;
using OpenAI_API;
using OpenAI_API.Models;
using OpenAI_API.Chat;
using System.Threading.Tasks;
public partial class ChatGD : Node
{
	OpenAIAPI api;

	Conversation chat;

	SaveManager saveManager;
	public override void _Ready()
	{
		saveManager = GetNode<SaveManager>("/root/saveManager");
		api = new OpenAIAPI(saveManager.GetAPIKey("ChatGPT"));
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