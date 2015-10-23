using System;

public interface ICrystallizeServer {
    ServerLogger Logger { get; }
    LeaderBoard LeaderBoard { get; }
    PlayerTransformTable TransformTable { get; }
    bool IsStarted { get; }

    void Close();
    void Initialize();
    void BroadcastTransforms();
}
