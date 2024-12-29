namespace SDL2.SDL_Extensions;

public class SDLPointer<T>(IntPtr value = 0) where T : IDisposable
{
    public IntPtr Value { get; set; } = value;
}

