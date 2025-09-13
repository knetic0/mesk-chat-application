import MESKButton from '@/components/mesk-button';
import Password from '@/components/password';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useRegisterMutation } from '@/features/commands/auth/register/handler';
import { registerSchema, type RegisterInput } from '@/features/commands/auth/register/schema';
import { useLanguage } from '@/hooks/use-language';
import { router } from '@/router';
import type { ResponseEntityOfEmptyResponse } from '@/types';
import { zodResolver } from '@hookform/resolvers/zod';
import { createFileRoute } from '@tanstack/react-router'
import { ArrowRight, Lock, Mail } from 'lucide-react';
import { useForm } from 'react-hook-form';

export const Route = createFileRoute('/auth/register')({
  component: RouteComponent,
})

function RouteComponent() {
  const { t } = useLanguage();

  const {
    register,
    handleSubmit,
    formState: { errors },
    setError,
    reset
  } = useForm<RegisterInput>({
    resolver: zodResolver(registerSchema)
  });

  const { mutate, isPending } = useRegisterMutation({
    onSuccess: (data: ResponseEntityOfEmptyResponse) => {
      if(data.isSuccess) {
        reset();
        router.navigate({ to: '/auth/login' });
      }
    },
    onError: (error: any) => {
        if (error?.response?.data?.errors) {
            const apiErrors = error.response.data.errors;
            Object.entries(apiErrors).forEach(([field, message]) => {
                setError(field as keyof RegisterInput, {
                    type: "server",
                    message: String(message),
                });
            });
        } else {
            setError("root", { type: "server", message: error.message || "Something went wrong" });
        }
    },
  })

  const onSubmit = (data: RegisterInput) => mutate(data);

  return (
    <>
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <div className="bg-gradient-to-r from-blue-600 to-indigo-600 w-16 h-16 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-lg">
            <Lock className="w-8 h-8 text-white" />
          </div>
          <h1 className="text-3xl font-bold mb-2">{t("welcome")}</h1>
          <p className="">{t("register.title")}</p>
        </div>
        <Card className="shadow-xl">
          <CardHeader className="space-y-1">
            <CardTitle className="text-2xl text-center">{t("register.title")}</CardTitle>
            <CardDescription className="text-center">
              {t("register.description")}
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
              <div className="space-y-2">
                <Label htmlFor="firstName">{t("firstName")}</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                  <Input
                    id="firstName"
                    type="text"
                    placeholder="John"
                    {...register("firstName")}
                    className="pl-9"
                    required
                  />
                </div>
                {errors.firstName && <p className="text-sm text-red-600 mt-1">{errors.firstName.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="lastName">{t("lastName")}</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                  <Input
                    id="lastName"
                    type="text"
                    placeholder="Doe"
                    {...register("lastName")}
                    className="pl-9"
                    required
                  />
                </div>
                {errors.lastName && <p className="text-sm text-red-600 mt-1">{errors.lastName.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="username">{t("username")}</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                  <Input
                    id="username"
                    type="text"
                    placeholder="john.doe"
                    {...register("username")}
                    className="pl-9"
                    required
                  />
                </div>
                {errors.username && <p className="text-sm text-red-600 mt-1">{errors.username.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="email">{t("email")}</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
                  <Input
                    id="email"
                    type="email"
                    placeholder="@email.com"
                    {...register("email")}
                    className="pl-9"
                    required
                  />
                </div>
                {errors.email && <p className="text-sm text-red-600 mt-1">{errors.email.message}</p>}
              </div>
              <div className="space-y-2">
                <Label htmlFor="password">{t("password")}</Label>
                <Password {...register("password")} />
                {errors.password && <p className="text-sm text-red-600 mt-1">{errors.password.message}</p>}
              </div>
              <MESKButton label={t("register.submit")} loading={isPending} loadingText={t("register.loading")} icon={<ArrowRight />} />
            </form>
          </CardContent>
          <CardFooter>
            <p className="px-8 text-center text-sm text-muted-foreground">
              {t("register.haveAccount")}{" "}
              <Button variant="link" className="px-0 font-normal" onClick={() => router.navigate({ to: '/auth/login' })}>
                {t("signIn")}
              </Button>
            </p>
          </CardFooter>
        </Card>
      </div>
    </>
  )
}
