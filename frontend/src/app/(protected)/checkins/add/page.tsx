"use client";

import CheckinForm from "@/components/CheckinForm";
import { useRouter } from "next/navigation";
import { Checkin } from "../[id]/edit/page";
import { useState } from "react";

export default function NewCheckinPage() {
  const router = useRouter();
  const [errorMessage, setError] = useState("");
  const handleSubmit = async (data: Checkin) => {
    const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;

    const res = await fetch(`${API_BASE}/checkins/`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data),
    });

    const success: boolean = await res.json();
    
    if (success) {
      router.push("/checkins");
    } else {
      setError("There was an error in creating, either you already submitted for today or an internal server occured");
    }
  };

  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-xl font-bold mb-4">New Check-in</h1>
      <CheckinForm onSubmit={handleSubmit} mode="create" />
      {errorMessage && (
        <p className="mt-2 text-sm text-gray-700">{errorMessage}</p>
      )}
    </div>
  );
}