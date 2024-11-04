using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Godot;
using System.Threading.Tasks;
using Azure.Core;




public partial class AzuirGD : Node
{
    // Called when the node enters the scene tree for the first time.




    SpeechConfig speechConfig;

    MasterScript master;

    SaveData saveData;

    public override void _Ready()
    {

        saveData = GetNode<SaveData>("/root/Data/SaveData");
        speechConfig = SpeechConfig.FromSubscription(saveData.GetAPIKey("Azuir"), saveData.GetAPIKey("AZReigon"));
        speechConfig.SpeechRecognitionLanguage = "en-US";
        speechConfig.SetProfanity(ProfanityOption.Raw);


    }



    public async Task<string> GetText(string recordingPath)
    {
        GD.Print(recordingPath);
        using var audioConfig = AudioConfig.FromWavFileInput(ProjectSettings.GlobalizePath(recordingPath));
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        if (OutputSpeechRecognitionResult(speechRecognitionResult))
        {

            return speechRecognitionResult.Text;
        }

        return null;


    }



    bool OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {

        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                GD.Print($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                return true;
            case ResultReason.NoMatch:
                GD.Print($"NOMATCH: Speech could not be recognized.");
                GetNode<NotificationsManager>("/root/Managers/Notification").NewNotification("error", $"[center]AZURE ERROR", $"[center]No speech could not be recognized", 10);
                return false;

            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                GD.Print($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    GD.Print($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    GD.Print($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    GetNode<NotificationsManager>("/root/Managers/Notifications").NewNotification("error", $"[center]AZURE ERROR", $"[center]Did you set the speech resource key and region values?", 10);
                    GD.Print($"CANCELED: Did you set the speech resource key and region values?");
                }
                return false;
        }
        return false;
    }




}
