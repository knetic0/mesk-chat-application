import { useLanguage } from '@/hooks/use-language';
import type { Status } from '@/types';

type StatusConfigValue = {
  key: string;
  color: string;
};

const STATUS_CONFIG: Record<Status, StatusConfigValue> = {
  0: { key: 'online', color: 'bg-green-500' },
  1: { key: 'away', color: 'bg-orange-500' },
  2: { key: 'offline', color: 'bg-gray-500' },
};

type StatusBadgeProps = {
  status: Status;
};

function StatusBadge({ status }: StatusBadgeProps) {
  const { t } = useLanguage();
  const _status: StatusConfigValue = STATUS_CONFIG[status] ?? {
    key: 'unknown',
    color: 'bg-gray-300',
  };

  return (
    <div className="flex gap-1 items-center">
      <div className={`w-2 h-2 rounded-full ${_status.color}`}></div>
      <div className="text-xs text-gray-400">{t(_status.key)}</div>
    </div>
  );
}

export default StatusBadge;
