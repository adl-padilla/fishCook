using static SDL2.SDL;
namespace SDL2.SDL_Extensions;
public class Texture : SDLPointer<Texture>, IDisposable
{
    public SDL_Point Center { get; }
    public uint Format { get; }
    public int Access { get; }
    public int Width { get; }
    public int Height { get; }

    public Texture(nint value = 0) : base(value)
    {
        int _ = SDL_QueryTexture(Value, out uint format, out int access, out int w, out int h);
        Format = format;
        Access = access;
        Width = w;
        Height = h;
        Center = new SDL_Point { x = (int)Math.Round(Width * .5), y = (int)Math.Round(Height * .5) };
    }

    public void Dispose()
    {
        SDL_DestroyTexture(Value);
        SDL_LogInfo(0, $"{DateTime.Now.TimeOfDay} [{nameof(Texture)}.{nameof(Dispose)}] Destroying {Convert.ToString(Value, 16)}");
        GC.SuppressFinalize(this);
    }
}

