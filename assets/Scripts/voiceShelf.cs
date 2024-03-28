using ElevenLabs.Voices;
using Godot;
using System;

public partial class voiceShelf : Node
{
    public Voice voice = null;

    public override void _Ready()
    {
        if (voice != null)
            GetNode<Button>("Button").Text = voice.Name;
        else
            GetNode<Button>("Button").Text = "Loading...";
    }

    public void ButtonPressed()
    {
        GetNode<MasterScript>("/root/Main Scene").Libraries.elevinLabs.currentVoice = voice;
        GD.Print($"Voice set to {GetNode<MasterScript>("/root/Main Scene").Libraries.elevinLabs.currentVoice.Name}");
    }

}
