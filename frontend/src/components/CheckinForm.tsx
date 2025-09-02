"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Checkin } from "@/app/(protected)/checkins/[id]/edit/page";
import { authService } from "@/lib/auth";

type CheckinFormProps = {
    initialData?: {
        id?: number;
        userId?: number;
        mood: number;
        createdAt: Date;
        notes?: string;
    };
    onSubmit: (data: Checkin) => Promise<void>;
    mode?: "create" | "update";
};

export default function CheckinForm({
    initialData,
    onSubmit,
    mode = "create",
}: CheckinFormProps) {
    const router = useRouter();
    const [mood, setMood] = useState(initialData?.mood ?? 3);
    const [userId, setUserId] = useState(initialData?.userId ?? JSON.parse(authService.getToken()!).id);
    const [createdAt, setDate] = useState(initialData?.createdAt ?? new Date().toISOString().split("T")[0])
    const [notes, setNotes] = useState(initialData?.notes ?? "");
    const [loading, setLoading] = useState(false);
    
    const formattedDate = createdAt.toString();
    console.log(formattedDate);
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        await onSubmit({id: initialData?.id ?? undefined, notes: notes, userId: userId, mood: mood});
        setLoading(false);
    };

    const goBack = () => {
        router.push("/checkins");
    }

    return (
        <form onSubmit={handleSubmit} className="p-4 space-y-4">
            <div>
                <label className="block text-sm font-medium">Mood (1–5)</label>
                <select
                    value={mood}
                    onChange={(e) => setMood(Number(e.target.value))}
                    className="mt-1 block w-full rounded-md border border-gray-300 p-2"
                >
                    {[1, 2, 3, 4, 5].map((m) => (
                        <option key={m} value={m}>
                            {m}
                        </option>
                    ))}
                </select>
            </div>

            <div>
                <label className="block text-sm font-medium">Date</label>
                <input
                    type="date"
                    value={formattedDate}
                    disabled
                    className="mt-1 block w-full rounded-md border border-gray-300 p-2"
                />
            </div>

            <div>
                <label className="block text-sm font-medium">Notes (optional)</label>
                <textarea
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                    className="mt-1 block w-full rounded-md border border-gray-300 p-2"
                />
            </div>

            <div className="flex justify-between">
                <button
                    type="submit"
                    disabled={loading}
                    className="rounded bg-blue-600 mr-4 px-4 py-2 text-white disabled:opacity-50 cursor-pointer"
                >
                    {loading ? "Saving..." : mode === "create" ? "Submit" : "Update"}
                </button>

                <button
                    type="button"
                    onClick={goBack}
                    disabled={loading}
                    className="rounded bg-red-600 px-4 py-2 text-white disabled:opacity-50 cursor-pointer"
                >
                    Cancel
                </button>
            </div>
        </form>
    );
}