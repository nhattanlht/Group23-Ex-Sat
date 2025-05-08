import { useState } from "react";

const EmailInput = ({ id, name, value="", required=false, disabled=false, onChange, ALLOWED_EMAIL_ENDING = null }) => {
    const [error, setError] = useState(null);

    const handleChange = (e) => {
        onChange(e);
        if (ALLOWED_EMAIL_ENDING && !e.target.value.endsWith(ALLOWED_EMAIL_ENDING)) {
            setError(`Email phải kết thúc bằng "${ALLOWED_EMAIL_ENDING}"`);
        } else {
            setError(""); // Clear the error if valid
        }
    }
    return (
        <div>
            <input
                id={id}
                name={name}
                type="email"
                value={value}
                onChange={handleChange}
                className={`w-full border p-2 ${error ? "border-red-500" : ""}`} // Highlight border if there's an error
                required={required}
                disabled={disabled}
            />
            {error && (
                <small className="text-red-500">{error}</small> // Display error message
            )}
        </div>
    );
}

export default EmailInput;