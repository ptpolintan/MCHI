"use client";

import React, { useState, useEffect } from "react";
import dynamic from "next/dynamic";
import type { TableColumn } from "react-data-table-component";
import { Checkin } from "@/app/(protected)/checkins/[id]/edit/page";
import { authService } from "../lib/auth";

type DataTableProps = {
  columns: TableColumn<Checkin>[];
  data: Checkin[];
  pagination?: boolean;
  highlightOnHover?: boolean;
  responsive?: boolean;
  total: number;
  page: number;
  pageSize: number;
  showUserColumn?: boolean;
  paginationServer: any;
  paginationTotalRows: number;
  paginationDefaultPage: number;
  paginationPerPage: number;
};

const DataTable = dynamic(() => import("react-data-table-component"), {
  ssr: false,
}) as any;

type Props = {
  checkins: Checkin[];
  onEdit?: (id: string) => void;
  showUserColumn?: boolean;
  total: number;
  page: number;
  pageSize: number;
  onPageChange: (page: number, newPerPage?: number) => void;
}

export default function CheckinsTable({
  checkins,
  onEdit,
  showUserColumn = false,
  total,
  page,
  pageSize,
  onPageChange
}: Props) {
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  const columns: TableColumn<Checkin>[] = [
    {
      name: "Actions",
      cell: (row) => (
        <div className="space-x-2">
          {onEdit && (
            <button
              hidden={row.userId != JSON.parse(authService.getToken()!).id}
              className="px-2 py-1 bg-blue-500 text-white rounded cursor-pointer"
              onClick={() => onEdit(row.id!.toString())}
            >
              Edit
            </button>
          )}
        </div>
      ),
    },
    ...(showUserColumn
      ? [
        {
          name: "User",
          selector: (row: Checkin) => row.user!.name,
        } as TableColumn<Checkin>,
      ]
      : []),
    {
      name: "Mood Level",
      selector: (row) => row.mood.toString(),
    },
    {
      name: "Notes",
      selector: (row) => row.notes || "",
    },
    {
      name: "Date Submitted",
      selector: (row) => row.createdAt!.toString(),
    }
  ];

  if (!mounted) {
    return (
      <div className="flex items-center justify-center h-40">
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  return (
    <DataTable
      columns={columns}
      data={checkins}
      pagination
      paginationServer
      paginationTotalRows={total}
      paginationDefaultPage={page}
      paginationPerPage={pageSize}
      onChangePage={(newPage: number) => onPageChange(newPage, pageSize)}
      onChangeRowsPerPage={(newPerPage: number, newPage: number) =>
        onPageChange(newPage, newPerPage)
      }
      highlightOnHover
      striped
      responsive
      progressPending={!checkins}
      fixedHeader
      fixedHeaderScrollHeight="400px"
    />
  );
}