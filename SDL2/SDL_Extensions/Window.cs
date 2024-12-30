using static SDL2.SDL;
namespace SDL2.SDL_Extensions;

public class Window : SDLPointer<Window>, IDisposable
{
    public readonly Renderer Renderer = new();

    public Window(nint value = 0) : base(value)
    {
        Renderer = new Renderer(SDL_CreateRenderer(Value,
          -1,
          SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
          SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC));
        if (Renderer.Value == IntPtr.Zero)
        {
            SDL_LogInfo(0, $"There was an issue creating the renderer. {SDL_GetError()}");
        }

    }

    public void Dispose()
    {
        Renderer.Dispose();
        SDL_DestroyWindow(Value);
        //SDL_LogInfo(0,$"Destroying Window {Value}");
        SDL_LogInfo(0, $"{DateTime.Now.TimeOfDay} [{nameof(Window)}.{nameof(Dispose)}] Destroying {Convert.ToString(Value, 16)}");
        GC.SuppressFinalize(this);
    }

    internal Texture LoadTexture(string fullName)
    {
        var texture = Renderer.LoadTexture(fullName);
        if (texture.Value == IntPtr.Zero)
        {
            SDL_LogInfo(0, $"There was an issue with creating texture. {SDL_GetError()} {fullName}");
        }
        return texture;
    }

    internal int RenderClear()
    {
        return Renderer.RenderClear();
    }

    internal int RenderTexture(Texture background, ref SDL_Rect r1, ref SDL_Rect r2)
    {
        return Renderer.RenderCopy(background, ref r1, ref r2);
    }

    internal int RenderCopyEx(Texture texture, ref SDL_Rect r1, ref SDL_Rect r2, float angle, SDL_Point center, SDL_RendererFlip sdlRenderFlip)
    {
        return Renderer.RenderCopyEx(texture, ref r1, ref r2, angle, center, sdlRenderFlip);
    }

    internal int RenderSprite(Sprite sprite, double scale = 1.0f)
    {
        return Renderer.RenderCopyScale(sprite.Texture, sprite.Loc, scale);
    }

    internal void RenderPresent()
    {
        Renderer.RenderPresent();
    }

    internal int SetRenderDrawColor(SDL_Color color)
    {
        return Renderer.SetRenderDrawColor(color);
    }
}

