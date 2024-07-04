"use client";
import { Pagination } from "flowbite-react";
import React, { useState } from "react";

type Props = {
  currenPage: number;
  pageCount: number;
  pageChanged: (page: number) => void;
};

export default function AppPagination({
  currenPage,
  pageCount,
  pageChanged,
}: Props) {
  return (
    <Pagination
      currentPage={currenPage}
      onPageChange={(e) => pageChanged(e)}
      totalPages={pageCount}
      layout="pagination"
      showIcons={true}
      className="text-blue-500 mb-5"
    />
  );
}
