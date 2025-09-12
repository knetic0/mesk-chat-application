import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@radix-ui/react-dropdown-menu";
import { Button } from "./ui/button";
import { Moon, Sun } from "lucide-react";
import { useTheme } from "@/hooks/use-theme";
import { useTranslation } from "react-i18next";

function ChangeTheme() {
    const { setTheme } = useTheme();
    const { t } = useTranslation();

    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button 
                    variant="ghost" 
                    size="icon" 
                    className="h-10 w-10 rounded-full bg-white/50 dark:bg-gray-700/50 backdrop-blur-sm hover:bg-white/70 dark:hover:bg-gray-600/70 transition-all duration-200 border border-gray-200/50 dark:border-gray-600/50"
                >
                    <Sun className="h-5 w-5 scale-100 rotate-0 transition-all duration-300 dark:scale-0 dark:-rotate-90" />
                    <Moon className="absolute h-5 w-5 scale-0 rotate-90 transition-all duration-300 dark:scale-100 dark:rotate-0" />
                    <span className="sr-only">Toggle theme</span>
                </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent 
            align="end" 
            className="w-36 bg-white/90 dark:bg-gray-800/90 backdrop-blur-xl rounded-2xl shadow-xl border border-gray-200/50 dark:border-gray-700/50 p-2 mt-2"
            >
                <DropdownMenuItem
                    onClick={() => setTheme('light')} 
                    className="cursor-pointer rounded-xl px-4 py-3 text-sm font-medium hover:bg-gray-100/80 dark:hover:bg-gray-700/80 transition-all duration-200"
                >
                    ☀️ {t('theme.lightMode')}
                </DropdownMenuItem>
                <DropdownMenuItem 
                    onClick={() => setTheme('dark')} 
                    className="cursor-pointer rounded-xl px-4 py-3 text-sm font-medium hover:bg-gray-100/80 dark:hover:bg-gray-700/80 transition-all duration-200"
                >
                    🌙 {t('theme.darkMode')}
                </DropdownMenuItem>
                <DropdownMenuItem 
                    onClick={() => setTheme('system')} 
                    className="cursor-pointer rounded-xl px-4 py-3 text-sm font-medium hover:bg-gray-100/80 dark:hover:bg-gray-700/80 transition-all duration-200"
                >
                    💻 {t('theme.system')}
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    )
}

export default ChangeTheme;