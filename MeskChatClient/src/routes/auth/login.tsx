import { createFileRoute } from "@tanstack/react-router"
import { Mail, Lock, ArrowRight, Loader2 } from "lucide-react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator"
import { useLanguage } from "@/hooks/use-language"
import { type ResponseEntityOfLoginCommandResponse } from "@/types";
import { loginSchema, type LoginInput } from "@/features/commands/auth/login/schema";
import { useLoginMutation } from "@/features/commands/auth/login/handler";
import { router } from "@/router";
import { useAuth } from "@/hooks/use-auth";
import Password from "@/components/password";

export const Route = createFileRoute('/auth/login')({
  component: RouteComponent,
})

function RouteComponent() {
  const { login } = useAuth();
  const { t } = useLanguage();

  const {
    register,
    handleSubmit,
    formState: { errors },
    setError,
    reset
  } = useForm<LoginInput>({
    resolver: zodResolver(loginSchema)
  });

  const { mutate, isPending } = useLoginMutation({
    onSuccess: (data: ResponseEntityOfLoginCommandResponse) => {
      if(data.isSuccess && data.data) {
        const { accessToken, refreshToken } = data.data;
        login(accessToken, refreshToken);
        reset();
        router.navigate({ to: '/chat/{-$receiverId}' });
      }
    },
    onError: (error: any) => {
        if (error?.response?.data?.errors) {
            const apiErrors = error.response.data.errors;
            Object.entries(apiErrors).forEach(([field, message]) => {
                setError(field as keyof LoginInput, {
                    type: "server",
                    message: String(message),
                });
            });
        } else {
            setError("root", { type: "server", message: error.message || "Something went wrong" });
        }
    },
  })

  const onSubmit = (data: LoginInput) => mutate(data);

  return (
    <>
      <div className="w-full max-w-md">
        {/* Logo/Brand Section */}
        <div className="text-center mb-8">
          <div className="bg-gradient-to-r from-blue-600 to-indigo-600 w-16 h-16 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-lg">
            <Lock className="w-8 h-8 text-white" />
          </div>
          <h1 className="text-3xl font-bold mb-2">{t("welcome")}</h1>
          <p className="">{t("login.title")}</p>
        </div>

        {/* Login Form */}
        <Card className="shadow-xl">
          <CardHeader className="space-y-1">
            <CardTitle className="text-2xl text-center">{t("login.title")}</CardTitle>
            <CardDescription className="text-center">
              {t("login.description")}
            </CardDescription>
          </CardHeader>
          
          <CardContent className="space-y-4">
            <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
              {/* Email Field */}
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

              {/* Password Field */}
              <div className="space-y-2">
                <Label htmlFor="password">{t("password")}</Label>
                <Password {...register("password")} />
                {errors.password && <p className="text-sm text-red-600 mt-1">{errors.password.message}</p>}
              </div>

              {/* Remember Me & Forgot Password */}
              <div className="flex items-center justify-between">
                <Button variant="link" className="px-0 font-normal">
                  {t("login.forgotPassword")}
                </Button>
              </div>

              {/* Login Button */}
              <Button
                disabled={isPending}
                className="w-full bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700"
              >
                {isPending ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    {t("login.loading")}
                  </>
                ) : (
                  <>
                    {t("login.submit")}
                    <ArrowRight className="ml-2 h-4 w-4" />
                  </>
                )}
              </Button>
            </form>

            {/* Divider */}
            <div className="relative">
              <div className="absolute inset-0 flex items-center">
                <Separator className="w-full" />
              </div>
              <div className="relative flex justify-center text-xs uppercase">
                <span className="bg-background px-2 text-muted-foreground">veya</span>
              </div>
            </div>

            {/* Social Login */}
            <div className="grid grid-cols-2 gap-4">
              <Button variant="outline" className="w-full">
                <svg className="w-4 h-4 mr-2" viewBox="0 0 24 24">
                  <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
                  <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
                  <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
                  <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
                </svg>
                Google
              </Button>
              
              <Button variant="outline" className="w-full">
                <svg className="w-4 h-4 mr-2" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M24 4.557c-.883.392-1.832.656-2.828.775 1.017-.609 1.798-1.574 2.165-2.724-.951.564-2.005.974-3.127 1.195-.897-.957-2.178-1.555-3.594-1.555-3.179 0-5.515 2.966-4.797 6.045-4.091-.205-7.719-2.165-10.148-5.144-1.29 2.213-.669 5.108 1.523 6.574-.806-.026-1.566-.247-2.229-.616-.054 2.281 1.581 4.415 3.949 4.89-.693.188-1.452.232-2.224.084.626 1.956 2.444 3.379 4.6 3.419-2.07 1.623-4.678 2.348-7.29 2.04 2.179 1.397 4.768 2.212 7.548 2.212 9.142 0 14.307-7.721 13.995-14.646.962-.695 1.797-1.562 2.457-2.549z"/>
                </svg>
                Twitter
              </Button>
            </div>
          </CardContent>

          <CardFooter>
            <p className="px-8 text-center text-sm text-muted-foreground">
              {t("login.dontHaveAccount")}{" "}
              <Button variant="link" className="px-0 font-normal" onClick={() => router.navigate({ to: '/auth/register' })}>
                {t("signUp")}
              </Button>
            </p>
          </CardFooter>
        </Card>
      </div>
    </>
  )
}