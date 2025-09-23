import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { Button } from '@/components/ui/button';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { Home, ArrowLeft, Search } from 'lucide-react';
import { Trans } from 'react-i18next';
import { useLanguage } from '@/hooks/use-language';
import { router } from '@/router';

export const Route = createFileRoute('/not-found')({
  component: NotFoundPage,
});

function NotFoundPage() {
  const { t } = useLanguage();
  const navigate = useNavigate();

  return (
    <>
      <div className="max-w-2xl w-full text-center space-y-8">
        <div className="relative">
          <div className="text-9xl font-bold text-slate-200 select-none">404</div>
          <div className="absolute inset-0 flex items-center justify-center">
            <div className="w-24 h-24 bg-slate-300 rounded-full flex items-center justify-center">
              <Search className="w-12 h-12 text-slate-600" />
            </div>
          </div>
        </div>
        <div className="space-y-4">
          <h1 className="text-4xl font-bold text-slate-800 dark:text-white">
            {t('notFound.title')}
          </h1>
          <p className="text-lg text-slate-600 max-w-md mx-auto dark:text-slate-300">
            {t('notFound.description')}
          </p>
        </div>
        <Alert className="max-w-md mx-auto border-amber-200 bg-amber-50">
          <AlertDescription className="text-amber-800">
            <strong>{t('hint')}:</strong> {t('notFound.hintDescription')}
          </AlertDescription>
        </Alert>
        <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
          <Button
            className="flex items-center gap-2 bg-slate-900 hover:bg-slate-800 text-white px-6 py-3"
            onClick={() => navigate({ to: '/chat' })}
          >
            <Home className="w-4 h-4" />
            {t('homepage')}
          </Button>
          <Button
            variant="outline"
            className="flex items-center gap-2 px-6 py-3 bg-white text-slate-900 dark:text-white"
            onClick={() => router.history.back()}
          >
            <ArrowLeft className="w-4 h-4" />
            {t('goBack')}
          </Button>
        </div>
        <div className="pt-8 text-center">
          <Trans
            i18nKey="notFound.helpDescription"
            components={{
              p: <p className="text-slate-500 text-sm dark:text-slate-400" />,
              button: (
                <button className="text-slate-700 hover:text-slate-900 font-medium underline dark:text-slate-400 dark:hover:text-slate-300" />
              ),
            }}
          />
        </div>
      </div>
    </>
  );
}
