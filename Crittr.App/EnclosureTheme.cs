using Crittr.Shared.Models.Enums;
using Microsoft.AspNetCore.Components;

namespace Crittr.App;

public static class EnclosureTheme
{
    public static string CanvasBg(EnclosureType t) => t switch
    {
        EnclosureType.Aquarium or EnclosureType.Tank
            => "from-blue-100 to-blue-300 dark:from-blue-950/40 dark:to-blue-900/30",
        EnclosureType.Paludarium
            => "from-teal-50 to-blue-200 dark:from-teal-950/30 dark:to-blue-900/20",
        EnclosureType.Aviary
            => "from-sky-50 to-sky-200 dark:from-sky-950/30 dark:to-sky-900/20",
        EnclosureType.Insectarium
            => "from-amber-50 to-stone-200 dark:from-amber-950/30 dark:to-stone-900/20",
        EnclosureType.Cage or EnclosureType.FreeRoamRoom
            => "from-slate-50 to-slate-200 dark:from-slate-950/30 dark:to-slate-900/20",
        EnclosureType.RackSystem or EnclosureType.Bin
            => "from-zinc-50 to-zinc-200 dark:from-zinc-950/30 dark:to-zinc-900/20",
        _ => "from-green-50 to-green-100 dark:from-green-950/30 dark:to-green-900/20",
    };

    public static string CanvasBorder(EnclosureType t) => t switch
    {
        EnclosureType.Aquarium or EnclosureType.Tank   => "border-blue-200 dark:border-blue-900",
        EnclosureType.Paludarium                        => "border-teal-200 dark:border-teal-900",
        EnclosureType.Aviary                            => "border-sky-200 dark:border-sky-900",
        EnclosureType.Insectarium                       => "border-amber-200 dark:border-amber-900",
        EnclosureType.Cage or EnclosureType.FreeRoamRoom => "border-slate-200 dark:border-slate-700",
        EnclosureType.RackSystem or EnclosureType.Bin   => "border-zinc-200 dark:border-zinc-700",
        _ => "border-green-100 dark:border-green-900",
    };

    public static string HeroBg(EnclosureType t) => t switch
    {
        EnclosureType.Aquarium or EnclosureType.Tank   => "from-blue-100 via-blue-50 to-white dark:from-blue-950/60 dark:via-blue-900/30 dark:to-gray-900",
        EnclosureType.Paludarium                        => "from-teal-100 via-teal-50 to-white dark:from-teal-950/60 dark:via-teal-900/30 dark:to-gray-900",
        EnclosureType.Aviary                            => "from-sky-100 via-sky-50 to-white dark:from-sky-950/60 dark:via-sky-900/30 dark:to-gray-900",
        EnclosureType.Insectarium                       => "from-amber-100 via-amber-50 to-white dark:from-amber-950/60 dark:via-amber-900/30 dark:to-gray-900",
        EnclosureType.Cage or EnclosureType.FreeRoamRoom => "from-slate-100 via-slate-50 to-white dark:from-slate-900 dark:via-slate-800 dark:to-gray-900",
        EnclosureType.RackSystem or EnclosureType.Bin   => "from-zinc-100 via-zinc-50 to-white dark:from-zinc-900 dark:via-zinc-800 dark:to-gray-900",
        _ => "from-green-100 via-green-50 to-white dark:from-green-950/60 dark:via-green-900/30 dark:to-gray-900",
    };

    public static string TypeBadge(EnclosureType t) => t switch
    {
        EnclosureType.Aquarium or EnclosureType.Tank   => "bg-blue-100 dark:bg-blue-900/40 text-blue-800 dark:text-blue-300 border border-blue-200 dark:border-blue-800",
        EnclosureType.Paludarium                        => "bg-teal-100 dark:bg-teal-900/40 text-teal-800 dark:text-teal-300 border border-teal-200 dark:border-teal-800",
        EnclosureType.Aviary                            => "bg-sky-100 dark:bg-sky-900/40 text-sky-800 dark:text-sky-300 border border-sky-200 dark:border-sky-800",
        EnclosureType.Insectarium                       => "bg-amber-100 dark:bg-amber-900/40 text-amber-800 dark:text-amber-300 border border-amber-200 dark:border-amber-800",
        EnclosureType.Cage or EnclosureType.FreeRoamRoom => "bg-slate-100 dark:bg-slate-700 text-slate-700 dark:text-slate-300 border border-slate-200 dark:border-slate-600",
        EnclosureType.RackSystem or EnclosureType.Bin   => "bg-zinc-100 dark:bg-zinc-700 text-zinc-700 dark:text-zinc-300 border border-zinc-200 dark:border-zinc-600",
        _ => "bg-green-100 dark:bg-green-900/40 text-green-800 dark:text-green-300 border border-green-200 dark:border-green-800",
    };

    public static MarkupString CanvasOverlay(EnclosureType t) => t switch
    {
        EnclosureType.Aquarium or EnclosureType.Tank => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-10 pointer-events-none\" viewBox=\"0 0 200 100\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<path d=\"M0 85 Q50 75 100 85 Q150 95 200 85 L200 100 L0 100 Z\" fill=\"#1d4ed8\"/>" +
            "<circle cx=\"30\" cy=\"55\" r=\"4\" fill=\"#60a5fa\" opacity=\"0.6\"/>" +
            "<circle cx=\"34\" cy=\"45\" r=\"3\" fill=\"#93c5fd\" opacity=\"0.5\"/>" +
            "<circle cx=\"80\" cy=\"40\" r=\"5\" fill=\"#60a5fa\" opacity=\"0.5\"/>" +
            "<circle cx=\"85\" cy=\"30\" r=\"3\" fill=\"#93c5fd\" opacity=\"0.4\"/>" +
            "<circle cx=\"150\" cy=\"50\" r=\"4\" fill=\"#60a5fa\" opacity=\"0.6\"/>" +
            "</svg>"),
        EnclosureType.Terrarium or EnclosureType.Vivarium or EnclosureType.RackSystem or EnclosureType.Bin => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-10 pointer-events-none\" viewBox=\"0 0 200 100\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<ellipse cx=\"40\" cy=\"92\" rx=\"25\" ry=\"10\" fill=\"#166534\"/>" +
            "<ellipse cx=\"110\" cy=\"95\" rx=\"20\" ry=\"8\" fill=\"#15803d\"/>" +
            "<ellipse cx=\"170\" cy=\"90\" rx=\"28\" ry=\"12\" fill=\"#166534\"/>" +
            "<path d=\"M55 80 Q60 60 70 75\" stroke=\"#4ade80\" stroke-width=\"2\" fill=\"none\"/>" +
            "<path d=\"M120 70 Q125 50 135 65\" stroke=\"#4ade80\" stroke-width=\"2\" fill=\"none\"/>" +
            "</svg>"),
        EnclosureType.Paludarium => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-10 pointer-events-none\" viewBox=\"0 0 200 100\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<path d=\"M0 70 Q50 60 100 70 Q150 80 200 70 L200 100 L0 100 Z\" fill=\"#0d9488\"/>" +
            "<ellipse cx=\"30\" cy=\"65\" rx=\"18\" ry=\"8\" fill=\"#166534\" opacity=\"0.8\"/>" +
            "<ellipse cx=\"160\" cy=\"62\" rx=\"22\" ry=\"9\" fill=\"#15803d\" opacity=\"0.8\"/>" +
            "</svg>"),
        EnclosureType.Aviary => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-10 pointer-events-none\" viewBox=\"0 0 200 100\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<line x1=\"10\" y1=\"60\" x2=\"90\" y2=\"55\" stroke=\"#0c4a6e\" stroke-width=\"3\" stroke-linecap=\"round\"/>" +
            "<line x1=\"110\" y1=\"50\" x2=\"190\" y2=\"45\" stroke=\"#0c4a6e\" stroke-width=\"3\" stroke-linecap=\"round\"/>" +
            "<line x1=\"30\" y1=\"80\" x2=\"140\" y2=\"75\" stroke=\"#0369a1\" stroke-width=\"2.5\" stroke-linecap=\"round\"/>" +
            "</svg>"),
        EnclosureType.Insectarium => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-10 pointer-events-none\" viewBox=\"0 0 200 100\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<line x1=\"20\" y1=\"0\" x2=\"20\" y2=\"100\" stroke=\"#78350f\" stroke-width=\"2.5\"/>" +
            "<line x1=\"45\" y1=\"0\" x2=\"45\" y2=\"100\" stroke=\"#92400e\" stroke-width=\"2\"/>" +
            "<line x1=\"70\" y1=\"0\" x2=\"70\" y2=\"100\" stroke=\"#78350f\" stroke-width=\"2.5\"/>" +
            "<line x1=\"95\" y1=\"0\" x2=\"95\" y2=\"100\" stroke=\"#92400e\" stroke-width=\"2\"/>" +
            "<line x1=\"120\" y1=\"0\" x2=\"120\" y2=\"100\" stroke=\"#78350f\" stroke-width=\"2.5\"/>" +
            "<line x1=\"145\" y1=\"0\" x2=\"145\" y2=\"100\" stroke=\"#92400e\" stroke-width=\"2\"/>" +
            "<line x1=\"170\" y1=\"0\" x2=\"170\" y2=\"100\" stroke=\"#78350f\" stroke-width=\"2.5\"/>" +
            "</svg>"),
        EnclosureType.Cage or EnclosureType.FreeRoamRoom => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-[0.07] pointer-events-none\" viewBox=\"0 0 200 100\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<line x1=\"0\" y1=\"25\" x2=\"200\" y2=\"25\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"0\" y1=\"50\" x2=\"200\" y2=\"50\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"0\" y1=\"75\" x2=\"200\" y2=\"75\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"33\" y1=\"0\" x2=\"33\" y2=\"100\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"66\" y1=\"0\" x2=\"66\" y2=\"100\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"100\" y1=\"0\" x2=\"100\" y2=\"100\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"133\" y1=\"0\" x2=\"133\" y2=\"100\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "<line x1=\"166\" y1=\"0\" x2=\"166\" y2=\"100\" stroke=\"#475569\" stroke-width=\"1.5\"/>" +
            "</svg>"),
        _ => new MarkupString(""),
    };

    public static MarkupString HeroOverlay(EnclosureType t) => t switch
    {
        EnclosureType.Aquarium or EnclosureType.Tank => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-[0.08] pointer-events-none\" viewBox=\"0 0 400 180\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<path d=\"M0 155 Q100 140 200 155 Q300 170 400 155 L400 180 L0 180 Z\" fill=\"#1d4ed8\"/>" +
            "<circle cx=\"60\" cy=\"90\" r=\"7\" fill=\"#60a5fa\" opacity=\"0.6\"/>" +
            "<circle cx=\"200\" cy=\"70\" r=\"8\" fill=\"#60a5fa\" opacity=\"0.5\"/>" +
            "<circle cx=\"330\" cy=\"85\" r=\"6\" fill=\"#60a5fa\" opacity=\"0.6\"/>" +
            "</svg>"),
        EnclosureType.Terrarium or EnclosureType.Vivarium => new MarkupString(
            "<svg class=\"absolute inset-0 w-full h-full opacity-[0.07] pointer-events-none\" viewBox=\"0 0 400 180\" " +
            "xmlns=\"http://www.w3.org/2000/svg\" preserveAspectRatio=\"xMidYMid slice\">" +
            "<ellipse cx=\"80\" cy=\"170\" rx=\"55\" ry=\"20\" fill=\"#166534\"/>" +
            "<ellipse cx=\"250\" cy=\"172\" rx=\"45\" ry=\"18\" fill=\"#15803d\"/>" +
            "<ellipse cx=\"370\" cy=\"168\" rx=\"50\" ry=\"22\" fill=\"#166534\"/>" +
            "<path d=\"M120 145 Q130 110 145 135\" stroke=\"#4ade80\" stroke-width=\"3\" fill=\"none\"/>" +
            "</svg>"),
        _ => new MarkupString(""),
    };
}
