import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@radix-ui/react-dropdown-menu';
import { Button } from './ui/button';
import { Languages } from 'lucide-react';
import { useLanguage } from '@/hooks/use-language';

function ChangeLanguage() {
  const { supportedLanguages, changeLanguage, isLanguageActive } = useLanguage();

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant="ghost"
          size="icon"
          className="h-10 w-10 rounded-full bg-white/50 dark:bg-gray-700/50 backdrop-blur-sm hover:bg-white/70 dark:hover:bg-gray-600/70 transition-all duration-200 border border-gray-200/50 dark:border-gray-600/50"
        >
          <Languages className="h-5 w-5" />
          <span className="sr-only">Change language</span>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent
        align="end"
        className="w-36 bg-white/90 dark:bg-gray-800/90 backdrop-blur-xl rounded-2xl shadow-xl border border-gray-200/50 dark:border-gray-700/50 p-2 mt-2"
      >
        {supportedLanguages.map(language => (
          <DropdownMenuItem
            key={language.code}
            onClick={() => changeLanguage(language.code)}
            className={`cursor-pointer rounded-xl px-4 py-3 text-sm font-medium hover:bg-gray-100/80 dark:hover:bg-gray-700/80 transition-all duration-200 ${
              isLanguageActive(language.code) ? 'bg-gray-100/60 dark:bg-gray-700/60' : ''
            }`}
          >
            {language.flag} {language.label}
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}

export default ChangeLanguage;
