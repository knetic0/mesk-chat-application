import { createFileRoute } from '@tanstack/react-router';
import { Mail, Lock, ArrowRight } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Button } from '@/components/ui/button';
import { useLanguage } from '@/hooks/use-language';
import { type ResponseEntityOfLoginCommandResponse } from '@/types';
import { loginSchema, type LoginInput } from '@/features/commands/auth/login/schema';
import { useLoginMutation } from '@/features/commands/auth/login/handler';
import { router } from '@/router';
import { useAuth } from '@/hooks/use-auth';
import Password from '@/components/password';
import MESKButton from '@/components/mesk-button';

export const Route = createFileRoute('/auth/login')({
  component: RouteComponent,
});

function RouteComponent() {
  const { login } = useAuth();
  const { t } = useLanguage();

  const {
    register,
    handleSubmit,
    formState: { errors },
    setError,
    reset,
  } = useForm<LoginInput>({
    resolver: zodResolver(loginSchema),
  });

  const { mutate, isPending } = useLoginMutation({
    onSuccess: (data: ResponseEntityOfLoginCommandResponse) => {
      if (data.isSuccess && data.data) {
        const { accessToken, refreshToken } = data.data;
        login(accessToken, refreshToken);
        reset();
        router.navigate({ to: '/chat' });
      }
    },
    onError: (error: any) => {
      if (error?.response?.data?.errors) {
        const apiErrors = error.response.data.errors;
        Object.entries(apiErrors).forEach(([field, message]) => {
          setError(field as keyof LoginInput, {
            type: 'server',
            message: String(message),
          });
        });
      } else {
        setError('root', { type: 'server', message: error.message || 'Something went wrong' });
      }
    },
  });

  const onSubmit = (data: LoginInput) => mutate(data);

  return (
    <>
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <div className="bg-gradient-to-r from-blue-600 to-indigo-600 w-16 h-16 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-lg">
            <Lock className="w-8 h-8 text-white" />
          </div>
          <h1 className="text-3xl font-bold mb-2">{t('welcome')}</h1>
          <p className="">{t('login.title')}</p>
        </div>
        <Card className="shadow-xl">
          <CardHeader className="space-y-1">
            <CardTitle className="text-2xl text-center">{t('login.title')}</CardTitle>
            <CardDescription className="text-center">{t('login.description')}</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
              <div className="space-y-2">
                <Label htmlFor="email">{t('email')}</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                  <Input
                    id="email"
                    type="email"
                    placeholder="@email.com"
                    {...register('email')}
                    className="pl-9"
                    required
                  />
                </div>
                {errors.email && (
                  <p className="text-sm text-red-600 mt-1">{errors.email.message}</p>
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="password">{t('password')}</Label>
                <Password {...register('password')} />
                {errors.password && (
                  <p className="text-sm text-red-600 mt-1">{errors.password.message}</p>
                )}
              </div>
              <div className="flex items-center justify-between">
                <Button variant="link" className="px-0 font-normal">
                  {t('login.forgotPassword')}
                </Button>
              </div>
              <MESKButton
                label={t('login.submit')}
                loading={isPending}
                loadingText={t('login.loading')}
                icon={<ArrowRight />}
              />
            </form>
          </CardContent>
          <CardFooter>
            <p className="mx-auto text-center text-sm text-muted-foreground">
              {t('login.dontHaveAccount')}{' '}
              <Button
                variant="link"
                className="px-0 font-normal"
                onClick={() => router.navigate({ to: '/auth/register' })}
              >
                {t('signUp')}
              </Button>
            </p>
          </CardFooter>
        </Card>
      </div>
    </>
  );
}
