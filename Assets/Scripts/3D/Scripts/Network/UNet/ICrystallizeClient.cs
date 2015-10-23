using System;

public interface ICrystallizeClient {
    PlayerTransformTable TransformTable { get; }

    bool Connected { get; }
    int ConnectionID { get; }

    void Initialize();
    void Reconnect();
    void Close();
    void RequestAudioClipFromServer(bool isMale, string text, Action<AudioKey> callback);
    void RequestLeaderboardFromServer(Action<LeaderBoardGameData> callback);
    void RequestNameFromServer(string name, Action<bool> callback);
    void RequestNameFromServer(UsernamePasswordPair pair, Action<bool> callback);
    void RequestPlayerDataFromServer(string name, Action<PlayerData> callback);
    void RequestPlayerDataFromServer(UsernamePasswordPair name, Action<PlayerData> callback);
    void RequestAvatarFromServer(int playerID, Action<PlayerAvatarData> callback);
    void RequestAllAvatarsFromServer();
    void SendLeaderBoardDataToServer();
    void SendLogMessageToServer(string message);
    void SendPlayerDataToServer();
    void SendPositionDataToServer();
    void SendChatToServer(string line, int mode);
    void SendEmoteToAll(int emoteType);
    void SendSpeechBubbleToAll(PhraseSequence phrase);
}
