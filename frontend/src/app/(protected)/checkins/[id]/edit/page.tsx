"use client";

import { useRouter } from "next/navigation";
import CheckinForm from "@/components/CheckinForm";
import useSWR from "swr";
import { use } from "react";
import { User } from "../../page";

const fetcher = (url: string) => fetch(url).then((res) => res.json());

export type Checkin = {
  id?: number;
  userId: number;
  mood: number;
  createdAt?: Date;
  notes: string;
  user?: User
}

export default function EditCheckinPage({ params }: { params: Promise<{ id: string }> }) {
  const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;

  const router = useRouter();
  const param = use(params);
  const { data: checkin, isLoading } = useSWR(`${API_BASE}/checkins/${param.id}`, fetcher);

  const handleUpdate = async (data: Checkin) => {
    await fetch(`/api/checkins/${param.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });
    router.push(`/checkins/${param.id}`);
  };

  if (isLoading) return <p>Loading...</p>;
  if (!checkin) return <p>Check-in not found</p>;

  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-xl font-bold mb-4">Edit Check-in</h1>
      <CheckinForm
        initialData={{ mood: checkin.mood, notes: checkin.notes, createdAt: checkin.createdAt }}
        onSubmit={handleUpdate}
        mode="update"
      />
    </div>
  );
}