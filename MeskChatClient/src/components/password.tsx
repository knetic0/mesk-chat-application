import { useState, forwardRef } from 'react';
import { Input } from './ui/input';
import { Eye, EyeOff, Lock } from 'lucide-react';
import { Button } from './ui/button';

type PasswordProps = React.ComponentProps<typeof Input>;

const Password = forwardRef<HTMLInputElement, PasswordProps>((props, ref) => {
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div className="relative">
      <Lock className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
      <Input
        {...props}
        ref={ref}
        id="password"
        type={showPassword ? 'text' : 'password'}
        placeholder="••••••••"
        className="pl-9 pr-9"
        required
      />
      <Button
        type="button"
        variant="ghost"
        size="sm"
        className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent"
        onClick={() => setShowPassword(!showPassword)}
      >
        {showPassword ? (
          <EyeOff className="h-4 w-4 text-muted-foreground" />
        ) : (
          <Eye className="h-4 w-4 text-muted-foreground" />
        )}
      </Button>
    </div>
  );
});

Password.displayName = 'Password';

export default Password;
