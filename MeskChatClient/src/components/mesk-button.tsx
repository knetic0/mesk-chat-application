import { forwardRef } from "react";
import { Button } from "./ui/button";
import { Loader2 } from "lucide-react";

type MESKButtonProps = React.ComponentProps<typeof Button> & {
  label: string;
  icon?: React.ReactNode;
  loading?: boolean;
  loadingText?: string;
};

const MESKButton = forwardRef<HTMLInputElement, MESKButtonProps>(({ label, loading = false, loadingText = "", icon, ...props }) => {
    return (
        <Button
            {...props}
            disabled={loading}
            className="w-full bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700"
            >
            {loading ? (
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
            ) : (
                icon
            )}
            {loading ? loadingText : label}
        </Button>
    );
});

MESKButton.displayName = "MESKButton";

export default MESKButton;