import type { ApplicationUser, ApplicationUser2 } from "@/types";

export const getProfilePhoto = (user: ApplicationUser | ApplicationUser2): string => {
    return user?.profilePhotoUrl
        || "https://thumbs.dreamstime.com/b/default-profile-picture-icon-high-resolution-high-resolution-default-profile-picture-icon-symbolizing-no-display-picture-360167031.jpg";
}