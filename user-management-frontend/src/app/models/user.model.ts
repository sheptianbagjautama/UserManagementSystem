export interface User {
  id?: number;          // opsional, karena saat create biasanya belum ada id
  username: string;
  password: string;
  fullName: string;
  gender: 'Male' | 'Female';
  birthDate: string;
  address: string;
  phone: string;
}
