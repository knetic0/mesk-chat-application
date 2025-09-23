import { Skeleton } from './ui/skeleton';

const UsersSkeletonLoader = () => {
  return (
    <div className="flex items-center space-x-4">
      <Skeleton className="h-12 w-12 rounded-full bg-gradient-to-r from-blue-200 to-blue-300 dark:from-blue-800 dark:to-blue-700" />
      <div className="space-y-2">
        <Skeleton className="h-4 w-[250px] bg-gradient-to-r from-blue-200 to-blue-300 dark:from-blue-800 dark:to-blue-700" />
        <Skeleton className="h-4 w-[200px] bg-gradient-to-r from-blue-200 to-blue-300 dark:from-blue-800 dark:to-blue-700" />
      </div>
    </div>
  );
};

export default UsersSkeletonLoader;
