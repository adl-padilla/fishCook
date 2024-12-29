using static SDL2.SDL;
namespace SDL2.SDL_Extensions;
public class Surface : SDLPointer<Surface>, IDisposable
{

    public SDL_Point Center { get; }

    public int Width { get; }
    public nint Format { get; }
    public int Height { get; }

    public unsafe Surface(nint value = 0) : base(value)
    {
        var s = (SDL_Surface*)Value;
        Height = s->h;
        Width = s->w;
        Format = s->format;
        Center = new SDL_Point { x = (int)Math.Round(s->w * .5), y = (int)Math.Round(s->h * .5) };
    }

    public void Dispose()
    {
        SDL_FreeSurface(Value);
        SDL_LogInfo(0, $"{DateTime.Now.TimeOfDay} [{nameof(Surface)}.{nameof(Dispose)}] Destroying {Convert.ToString(Value, 16)}");
        GC.SuppressFinalize(this);
    }
}

