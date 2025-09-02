"use client";
import React from 'react';

import CheckinsTable from "../../../components/CheckinsTable";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import useSWR from "swr";
import { authService } from "../../../lib/auth";

const users = [
    { id: 1, name: "Alice" },
    { id: 2, name: "Bob" },
    { id: 3, name: "Charlie" },
];

export interface User {
    id: number;
    name: string;
    role: 1 | 2;
}

const fetcher = async (url: string) => {
    const res = await fetch(url, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "CurrentUserId": `${JSON.parse(authService.getToken()!).id}`,
            "CurrentUserRole": `${JSON.parse(authService.getToken()!).role}`
        }
    });

    const json = await res.json();

    return {
        data: json.data ?? [],
        totalRecords: json.totalRecords ?? 0
    };
};

export default function EmployeeCheckinsPage() {
    const router = useRouter();
    const user = JSON.parse(authService.getToken()!);
    const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;

    const [formFrom, setFrom] = useState<string>("");
    const [formTo, setTo] = useState<string>("");
    const [formUserId, setFormUserId] = useState<number | null>(null);

    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [total, setTotal] = useState(0);

    const [filters, setFilters] = useState({
        from: "",
        to: "",
        userId: null as number | null,
    });

    const params = new URLSearchParams({
        ...(filters.from && { from: filters.from }),
        ...(filters.to && { to: filters.to }),
        ...(filters.userId ? { userId: String(filters.userId) } : {}),
    }).toString();

    useEffect(() => {
        setFrom(new Date(new Date().setDate(new Date().getDate() - 7)).toISOString().split("T")[0]);
        setTo(new Date().toISOString().split("T")[0]);
        setFormUserId(user.role == 2 ? null : user.id);
    }, []);

    const { data: checkins, isLoading } = useSWR(
        `${API_BASE}/checkins?skip=${page}&take=${pageSize}&${params.toString()}`,
        fetcher
    );

    useEffect(() => {
        if (checkins) setTotal(checkins.totalRecords);
    }, [checkins]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setFilters({
            from: formFrom,
            to: formTo,
            userId: formUserId,
        });
    }

    const handlePageChange = (newPage: number, newPageSize?: number) => {
        if (newPageSize && newPageSize !== pageSize) {
            setPageSize(newPageSize);
            setPage(1);
        } else {
            setPage(newPage);
        }
    };

    return (
        <div className="p-6">
            <div className="mb-6">
                {/* Header + Add Check-in button */}
                <div className="flex justify-between items-center mb-4">
                    <h1 className="text-xl font-bold">My Check-ins</h1>
                    <button
                        onClick={() => router.push("/checkins/add")}
                        className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700"
                    >
                        + Add Check-in
                    </button>
                </div>

                {/* Filters + Search button */}
                <form
                    onSubmit={handleSubmit}
                    className="mb-6 grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-4 items-end"
                >
                    {/* From Date */}
                    <div className="flex flex-col">
                        <label htmlFor="from" className="text-sm font-medium text-gray-700">
                            From
                        </label>
                        <input
                            id="from"
                            type="date"
                            value={formFrom}
                            onChange={(e) => setFrom(e.target.value)}
                            className="mt-1 p-2 border rounded-lg"
                        />
                    </div>

                    {/* To Date */}
                    <div className="flex flex-col">
                        <label htmlFor="to" className="text-sm font-medium text-gray-700">
                            To
                        </label>
                        <input
                            id="to"
                            type="date"
                            value={formTo}
                            onChange={(e) => setTo(e.target.value)}
                            className="mt-1 p-2 border rounded-lg"
                        />
                    </div>

                    {/* User ID */}
                    {user?.role === 2 && (
                        <div className="flex flex-col">
                            <label htmlFor="userId" className="text-sm font-medium text-gray-700">
                                User
                            </label>
                            <select
                                id="userId"
                                value={formUserId ?? 0}
                                onChange={(e) => setFormUserId(Number(e.target.value))}
                                className="mt-1 p-2 border rounded-lg"
                            >
                                <option value={0}>All Users</option>
                                {users.map((u) => (
                                    <option key={u.id} value={u.id}>
                                        {u.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                    )}

                    {/* Search button next to filters */}
                    <div className="flex items-end">
                        <button
                            type="submit"
                            onClick={handleSubmit}
                            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
                        >
                            Search
                        </button>
                    </div>
                </form>
            </div>


            <CheckinsTable
                checkins={checkins?.data}
                total={total}
                page={page}
                pageSize={pageSize}
                onPageChange={handlePageChange}
                showUserColumn={true}
                onEdit={(id) => router.push(`/checkins/${id}/edit`)}
            />
            {isLoading && (
                <div className="mt-4 text-gray-500 text-sm">Loading...</div>
            )}
        </div>
    );
}