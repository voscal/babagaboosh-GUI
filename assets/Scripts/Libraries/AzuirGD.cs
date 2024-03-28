using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Godot;
using System.Threading.Tasks;
using System;




public partial class AzuirGD : Node
{
    // Called when the node enters the scene tree for the first time.


    static string speechRegion = "australiaeast";

    SpeechConfig speechConfig;

    MasterScript master;

    SaveManager saveManager;

    public override void _Ready()
    {
        saveManager = GetNode<SaveManager>("/root/saveManager");
        speechConfig = SpeechConfig.FromSubscription(saveManager.GetAPIKey("Azuir"), speechRegion);
        speechConfig.SpeechRecognitionLanguage = "en-US";
        speechConfig.SetProfanity(ProfanityOption.Raw);


    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public async Task<string> GetTextFromWav(string recordingPath)
    {




        using var audioConfig = AudioConfig.FromWavFileInput(recordingPath);
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        if (OutputSpeechRecognitionResult(speechRecognitionResult))
        {
            return speechRecognitionResult.Text;
        }

        return null;


    }



    static bool OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                GD.Print($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                return true;
            case ResultReason.NoMatch:
                GD.Print($"NOMATCH: Speech could not be recognized.");
                return false;

            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                GD.Print($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    GD.Print($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    GD.Print($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    GD.Print($"CANCELED: Did you set the speech resource key and region values?");
                }
                return false;
        }
        return false;
    }

}
